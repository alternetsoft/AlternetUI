var xmldb_vers = "1.0";
var xml_ready = false;
var szRowState = "RowState",szUni = "string.uni",szNested = "nested",szTrue = "true",szFalse = "false",szDel = "2",szNew = "4",szOrg = "1",szMod = "8",szDetUpd = "64";
var szErr_HasDets = "Cannot delete master record with details.";
var szErr_Invalid = "Invalid action";
var DecPoint = ".";var s = (25 / 10).toString();var p = s.indexOf(".");DecPoint = p > 0 ? ".":",";

new xmlRowSet(null, null, null);
xmlRowSet.prototype.parent = null;
xmlRowSet.prototype.linkFld = null;
xmlRowSet.prototype.pDets = null;
xmlRowSet.prototype.noDel = 0;xmlRowSet.prototype.noIns = 0;xmlRowSet.prototype.noUpd = 0;
//private methods
xmlRowSet.prototype.InitRowSet = InitRowSet;
xmlRowSet.prototype.notify = function(reason)
	{
		if (this.regobjs == null)
			return;
		var j;
		for(j = 0;j < this.regobjs.length;j++)
			this.regobjs[j].refr(reason);
	}
xmlRowSet.prototype.forcepost = DsForcePost;
xmlRowSet.prototype.regobj = function(obj)
	{
		if(this.regobjs == null)
			this.regobjs = new Array();
		this.regobjs[this.regobjs.length] = obj;
	}
xmlRowSet.prototype.DeltaChanges = null;
xmlRowSet.prototype.resetDets = resetDets;
xmlRowSet.prototype.del = delRow;
xmlRowSet.prototype.ins = insRow;
xmlRowSet.prototype.upd = updRow;
//public functions
xmlRowSet.prototype.first = function()
	{
		return this.setPos(0);
	}
xmlRowSet.prototype.next = function()
	{
		return this.setPos(this.pos + 1);
	}
xmlRowSet.prototype.prev = function()
	{
		return this.setPos(this.pos - 1);
	}
xmlRowSet.prototype.last = function()
	{
		return this.setPos(this.RowCnt - 1);
	}
xmlRowSet.prototype.setPos = function(pos)
	{
		if(pos >= this.RowCnt || pos < 0 || this.RowCnt == 0)
		{
			this.resetDets();
			return 1;
		}
		if(this.pos != pos)
		{
			this.pos = pos;
			this.resetDets();
		} 
		this.notify(0);return 0;
	}
xmlRowSet.prototype.getRow = function(pos)
	{
		return this.idx.row(pos);
	}
xmlRowSet.prototype.makeRow = makeRow;
xmlRowSet.prototype.RowState = RowState;
xmlRowSet.prototype.insert = dsInsert;
xmlRowSet.prototype.modify = dsModify;
xmlRowSet.prototype.deletex = dsDelete;
xmlRowSet.prototype.undo = dsUndo;
xmlRowSet.prototype.sort = function(n)
	{
		this.idx.sort(n);
		this.first();
		this.notify(1);
	}
xmlRowSet.prototype.getDelta = function()
	{
		return this.DeltaChanges.make();
	}
xmlRowSet.prototype.Apply = null;
xmlRowSet.prototype.MakePermanent = function()
	{
		this.DeltaChanges.reset();
		this.notify(1);
		this.resetDets();
	}
xmlRowSet.prototype.BeforePost = function(a, r)
	{
		return r;
	}
xmlRowSet.prototype.AfterPost = function(a, r)
	{
		return r;
	}
xmlRowSet.prototype.OnError = function(s)
	{
		alert(s);return 1;
	}
xmlRowSet.prototype.OnNewRow = function()
	{
		return null;
	}

function xmlRowSet(doc, parent, linkFld) 
{
	this.doc = doc;
	if(doc == null){if(parent != null)this.doc = parent.doc; else return null;}
	if(this.doc == ""){if(parent == null)alert("Invalid XML-packet."); return null;}
	this.root = null;
	if(parent != null){this.parent = parent;this.linkFld = linkFld;this.InitRowSet = InitDetailRowSet;}
	this.MetaData = null;this.RowData = null;this.FieldCnt = 0;this.FieldData = null;
	//public:
	this.pos = 0;this.RowCnt = 0;
	this.Fields = new Fields();
	//Init
	this.InitRowSet();
	this.pos = -1;
	this.first();
	return this;
}

function InitRowSet()
{
	if(this.doc == null || this.doc == "")return;
	var i;
	this.root = this.doc.documentElement;
	if(this.root == null)return;
	if(this.root.tagName == "DATAPACKET"){
		this.MetaData = this.root.childNodes.item(0); 
		this.RowData = this.root.childNodes.item(1);
		this.RowCnt = this.RowData.childNodes.length;
		this.FieldData = this.MetaData.childNodes.item(0);
		this.FieldCnt = this.FieldData.childNodes.length;
		var des;
		for(i = 0;i < this.FieldCnt;i++)
		{
			des = new FldDes(this, this.FieldData.childNodes.item(i));if(des)this.Fields.Add(des);
		} 
	}
	else{this.RowData = this.root;this.RowCnt = this.RowData.childNodes.length;}

	var params = this.MetaData.childNodes.item(1);

	if(params.getAttribute("READONLY") != null){this.noDel = 1;this.noIns = 1;this.noUpd = 1;}
	for(i = 0;i < params.childNodes.length;i++)
	{
		var p = params.childNodes.item(i);
		var n = p.getAttribute("Name")
			;var v = p.getAttribute("Value");
		if(n != null && v != null)
			if(n == "DISABLE_DELETES")
				this.noDel = 1;
			else if	(n == "DISABLE_INSERTS")
				this.noIns = 1;
			else if (n == "DISABLE_EDITS")
				this.noUpd = 1;
	}
	this.DeltaChanges = new DeltaChanges(this);
	this.Apply = dsApply;
	this.idx = new idx(this);
}

function InitDetailRowSet()
{
	var i;
	var parent = this.parent;
	var fd = parent.FieldData.childNodes;
	var l = fd.length; 
	this.FieldData = parent.Fields.Field[this.linkFld].node.childNodes.item(0);
	if(this.FieldData == null)return;
	this.FieldCnt = this.FieldData.childNodes.length;  
	var des;
	for(i = 0;i < this.FieldCnt;i++)
	{
		des = new FldDes(this, this.FieldData.childNodes.item(i));
		if(des)
			this.Fields.Add(des);
	}
	if(parent.pDets == null)
		parent.pDets = new Array();
	parent.pDets[parent.pDets.length] = this;
	this.DeltaChanges = parent.DeltaChanges;
	parent.resetDets();
	this.idx = new idx(this);
	this.noDel = parent.noDel;this.noIns = parent.noIns;this.noUpd = parent.noUpd;
}

function findcnode(n, t)
{
	if(n == null)return null;
	var j,l = n.childNodes.length;
	for(j = 0;j < l;j++)if(n.childNodes.item(j).tagName == t)return n.childNodes.item(j);
	return null;
}

function resetDets()
{
	if (this.pDets == null) return;
	var i,l = this.pDets.length;
	for(i = 0;i < l;i++)
	{
		var rsd = this.pDets[i];
		var row = this.idx.row(this.pos);
		var det = findcnode(row, rsd.linkFld);
		rsd.RowData = det;
		rsd.RowCnt = (det == null) ? 0:det.childNodes.length;
		if(rsd.idx) rsd.idx.sort(null);
		rsd.first();
		rsd.resetDets();
		rsd.notify(2);
	}
}

function dsApply(frm, el)
{
	if(frm == null || el == null || this.forcepost() != 0 || this.DeltaChanges.row.length == 0)return;
	var delta = this.getDelta();
	if(delta.childNodes.item(1).childNodes.length == 0) return;
	el.value = (delta.xml != null) ? delta.xml:delta.xmlstr();
	frm.submit();
}

function dsInsert(prow, bFollow)
{
	if (this.noIns){return this.OnError(szErr_Invalid);}
	prow = this.BeforePost(1, prow);
	if(prow == null) return 1;
	var spos = this.pos;
	var pN = this.ins();
	if(pN == null) return 1;
	var r = this.idx.row(this.pos);
	r.setAttribute(szRowState, szNew);
	this.upd(prow);

	if(this.pDets){
		var i,l = this.Fields.Cnt;
		for(i = 0;i < l;i++){if(this.Fields.Fieldx[i].Type == szNested){var el = this.doc.createElement(this.Fields.Fieldx[i].iname);r.appendChild(el);}}
	}

	if (bFollow == 0)this.pos = spos;
	this.resetDets();
	this.DeltaChanges.add(szNew, pN, null, this);
	this.idx.chg = 1;
	this.AfterPost(1, prow);
	return 0;
}

function dsModify(p, prow)
{
	if(this.noUpd){return this.OnError(szErr_Invalid);}
	prow = this.BeforePost(3, prow);
	if(prow == null) return 1;
	var pO,pM; 
	var spos = this.pos; this.pos = p;
	var i,l;
	pM = this.idx.row(p);
	var rstate = pM.getAttribute(szRowState);
	if(rstate != szNew){
		pO = pM.cloneNode(0);l = pM.childNodes.length;
		for(i = 0;i < l;i++)pO.appendChild(pM.childNodes.item(i).cloneNode(0));
		pM.setAttribute(szRowState, szMod);
		this.upd(prow);
		this.DeltaChanges.add(szMod, pM, pO, this);
	}else this.upd(prow); 
	this.pos = spos;
	this.idx.chg = 1;
	this.AfterPost(3, prow);
	return 0;
}

function dsDelete(p)
{
	if(this.noDel){return this.OnError(szErr_Invalid);}
	var r = this.idx.row(p); 
	r = this.BeforePost(2, r);
	if(r == null)return 1;
	var spos = this.pos; this.pos = p;
	var i,l = r.childNodes.length;
	if(l && this.pDets)
		for(i = 0;i < this.pDets.length;i++)
		{
			var det = findcnode(r, this.pDets[i].linkFld);
			if(det && det.childNodes.length > 0)
				return this.OnError(szErr_HasDets);
		}
	r.setAttribute(szRowState, szDel);
	var pD = this.del();
	if(p < spos) this.pos = spos - 1;
	this.resetDets();
	this.DeltaChanges.add(szDel, pD, null, this);
	this.idx.chg = 1;
	this.AfterPost(2, null);
	return 0;
}

function updRow(prow)
{ 
	var i,f,v,r = this.idx.row(this.pos);
	if(r == null)return r;
	for(i = 0;i < this.Fields.Cnt;i++)
	{
		f = this.Fields.Fieldx[i];
		v = prow[f.name];
		if (v != null) 
			f.put(r, v);
	}
	this.notify(1);
	return r;
}

function makeRow()
{
	var i,f,v,r = this.idx.row(this.pos);
	if(r == null)return r;
	var RowBuf = new Array();
	for(i = 0;i < this.Fields.Cnt;i++)
	{
		f = this.Fields.Fieldx[i];
		v = null;
		if(f.bAsAttr)
			v = r.getAttribute(f.iname);
		if(v != null)
			RowBuf[f.name] = v;
	}
	return RowBuf;
}

function insRow()
{
	if(this.FieldData == null)return null;
	var rname = "ROW";
	def = this.OnNewRow();
	if(this.linkFld)rname += this.linkFld;
	var r = this.doc.createElement(rname);
	var i,f;
	for(i = 0;i < this.Fields.Cnt;i++)
	{    
		var F;
		f = this.Fields.Fieldx[i];
		if(f.bAsAttr == 0)
		{
			var T = this.doc.createTextNode("");
			F = this.doc.createElement(f.iname);
			F.appendChild(T); r.appendChild(F);
		}
		else if(def && def[f.name] != null)
			r.setAttribute(def[f.name]);
	}
	if(this.RowData == null)
		this.RowData = this.doc.createElement(this.linkFld);
	this.RowData.appendChild(r);
	this.RowCnt++;
	this.pos = this.RowCnt - 1;
	this.idx.add(this.pos);
	return r;
}

function delRow()
{
	var r = this.idx.row(this.pos);
	if(r == null)return null;
	var pos = this.pos;
	this.RowData.removeChild(r);
	this.idx.rem(pos);
	if(pos == this.RowCnt - 1 && pos > 0)
		this.pos = pos - 1;
	this.RowCnt--;      
	this.notify(1); 
	return r;
}

function RowState(pos)
{
	var r = this.idx.row(pos);
	if(r == null)return "";
	var st = r.getAttribute(szRowState);
	if(st == null || st == "")return "";
	var v = parseInt(st, 10);
	if(v == 2)
		st = "D";
	if(v == 4)
		st = "I";
	if(v == 8)
		st = "M";
	if(v == 64)
		st = "DU";
	return st;
}

function Fields()
{
	//public:
	this.Field = new Array();
	this.Fieldx = new Array();
	this.Add = addFld;
	this.Cnt = 0;
}

function addFld(fldDes)
{
	var n = fldDes.name;
	if(n != "")
	{
		this.Field[n] = fldDes;
		this.Fieldx[this.Cnt] = fldDes;
		this.Cnt++;
	}
}

new FldDes(null, null);
FldDes.prototype.bAsAttr = 1;
FldDes.prototype.name = "";
FldDes.prototype.iname = "";
FldDes.prototype.Type = "string";
FldDes.prototype.readonly = 0;
FldDes.prototype.required = 0;
FldDes.prototype.maxlength = 0;
FldDes.prototype.subtype = null;
FldDes.prototype.decimals = null;
FldDes.prototype.fixeddec = null;
FldDes.prototype.currencySymbol = null;
FldDes.prototype.minval = null;
FldDes.prototype.maxval = null;
FldDes.prototype.defval = "";
FldDes.prototype.minmax = minmax;
FldDes.prototype.validate = Validate;
FldDes.prototype.valtype = function(v)
	{
		if((this.errNo = this.minmax(v, this.minval, this.maxval)) != 0) 
			return 0;
		return v;
	}
FldDes.prototype.valcomp = function(v1, v2)
	{
		if(v1 == v2) 
			return 0;
		return (v1 > v2) ? 1:-1;
	}
FldDes.prototype.todisp = function(v)
	{
		return v;
	}
FldDes.prototype.frdisp = function(v)
	{
		return v;
	}
FldDes.prototype.Value = rsValue;
FldDes.prototype.get = getValue;
FldDes.prototype.put = putValue;
FldDes.prototype.notNull = function(v)
	{
		if(v == "")
			return null;
		return v;
	}
FldDes.prototype.errNo = 0;

function FldDes(rs, node)
{
	this.rs = rs;
	if(node == null)return;
	this.node = node;
	var t = node.getAttribute("fieldname"),ti = node.getAttribute("attrname");
	if(ti == null){ti = node.getAttribute("tagname");this.bAsAttr = 0};
	this.iname = ti;
	this.name = (t == null) ? ti:t;
	t = node.getAttribute("fieldtype");
	if(t)this.Type = t;
	t = node.getAttribute("readonly");if(t)this.readonly = 1;
	t = node.getAttribute("required");if(t && this.readonly == 0) this.required = 1;
	t = node.getAttribute("WIDTH");if(t)this.maxlength = parseInt(t, 10);
	t = node.getAttribute("SUBTYPE");if(t)this.subtype = t;
	t = node.getAttribute("DECIMALS");if(t)this.decimals = parseInt(t, 10);
	if(this.subtype == "Money"){this.decimals = 4;}
	if(this.Type == "fixed"){this.fixeddec = this.decimals;}
	if(this.subtype == "Text")this.maxlength = 0;

	if(this.Type == "string" || this.Type == szUni){this.notNull = function(v){return v;};}else
		if(this.Type == "i1"){this.valtype = valint; this.valcomp = cmpint; this.minval = parseInt("-128");this.maxval = parseInt("127"); }else
		if(this.Type == "i2"){this.valtype = valint; this.valcomp = cmpint; this.minval = parseInt("-32768");this.maxval = parseInt("32767");}else
		if(this.Type == "i4"){this.valtype = valint; this.valcomp = cmpint; this.minval = parseInt("-2147483648");this.maxval = parseInt("2147483647")}else
		if(this.Type == "r8" || this.Type == "fixed"){this.valtype = valfloat; this.valcomp = cmpfloat;this.todisp = dispfloat;this.frdisp = xmlfloat;}else
		if(this.Type == "date"){this.valtype = valdate;this.valcomp = cmpstr;this.todisp = dispdate;this.frdisp = xmldatetime}else
		if(this.Type == "dateTime"){this.valtype = valdatetime;this.valcomp = cmpstr;this.todisp = dispdatetime;this.frdisp = xmldatetime}else
		if(this.Type == "time"){this.valtype = valtime;this.todisp = disptime;}else
		if(this.Type == "boolean"){this.valtype = valbool;this.todisp = dispbool;this.frdisp = dispbool;}

	t = node.getAttribute("MINVALUE");if(t)this.minval = t;
	t = node.getAttribute("MAXVALUE");if(t)this.maxval = t;
	for(i = 0;i < node.childNodes.length;i++){
		var p = node.childNodes.item(i);var n = p.getAttribute("Name");var v = p.getAttribute("Value");
		if(n != null && v != null)if(n == "MINVALUE") this.minval = v;else if(n == "MAXVALUE"){this.maxval = v;}
	}
}

function valbool(v)
{
	return v;
}
function dispbool(v)
{
	if(v == "")return v;
	if((v.toLowerCase()).indexOf(szTrue) >= 0) v = szTrue;else v = szFalse;
	return v;
}

function dispdatetime(v)
{
	if(v.length >= 8){var y = parseInt(v.substring(0, 4), 10),m = parseInt(v.substring(4, 6), 10) - 1,d = parseInt(v.substring(6, 8), 10);
		var t = v.indexOf("T");
		var D;
		if(t == -1)D = new Date(y, m, d);else
		{
			var h = 0,mi = 0,s = 0,ms = 0;
			v = v.substring(t + 1, v.length);
			t = v.indexOf(":");
			h = parseInt(v.substring(0, t), 10);
			v = v.substring(t + 1, v.length);
			t = v.indexOf(":");
			mi = parseInt(v.substring(0, t), 10);
			v = v.substring(t + 1, v.length);
			t = v.indexOf(":");
			s = parseInt(v.substring(0, 2), 10);
			ms = parseInt(v.substring(2, v.length), 10);
			D = new Date(y, m, d, h, mi, s, ms);
		}
		return D.toLocaleString();
	}
	return "";
}

function dispdate(v)
{
	if (v.length < 8)return "";
	var y = parseInt(v.substring(0, 4), 10);m = parseInt(v.substring(4, 6), 10) - 1;d = parseInt(v.substring(6, 8), 10);
	var D;
	D = new Date(y, m, d);
	var s = D.toLocaleString();
	var st = s.indexOf("00:");
	if(st > 0) s = s.substring(0, st);
	return s;
}

function disptime(v)
{
	return v;
}
function cntchrs(v)
{
	var s = new Array(),l = v.length;
	s = v.split("&#");
	l -= (s.length - 1) * 5;
	return l > 0 ? l:0;
}
function Validate(v)
{
	var err = "";
	if(this.required && v == ""){err = this.name + " :Value is required";}else
		if(this.maxlength && v.length > this.maxlength)
	{
		if(cntchrs(v) > this.maxlength) 
			err = this.name + " :Value is too long";
	}
	if(err != "")
	{
		this.rs.OnError(err);
		return null;
	}
	v = this.valtype(v);
	if(v != null)
	{
		var s = this.frdisp(v);
		if(!s) 
			return s;
		v = this.todisp(s);
	}
	return v;
}

function cmpint(v1, v2)
{
	if(v1 == v2)return 0;
	var i1 = parseInt(v1, 10),i2 = parseInt(v2, 10);
	if(isNaN(i1) || isNaN(i2)) return cmpstr(v1, v2);
	return i1 - i2;
}

function cmpfloat(v1, v2)
{
	if(v1 == v2) return 0;
	var f1 = parseFloat(v1),f2 = parseFloat(v2);
	if(isNaN(f1) || isNaN(f2)) return cmpstr(v1, v2);
	return f1 - f2;
}


function cmpstr(v1, v2)
{
	if(v1 == v2)return 0;
	if(v1 == null)return -1;
	if(v2 == null)return 1;
	return (v1 > v2) ? 1:-1;
}

function valint(v)
{
	if(v != ""){
		var i = parseInt(v, 10); 
		if(isNaN(i)) {this.errNo = 3;this.rs.OnError(this.name + " : Invalid integer");return null;}
		if((this.errNo = this.minmax(i, this.minval, this.maxval)) != 0) return null;
		v = i.toString();
	}
	this.errNo = 0;
	return v;
}

function minmax(i, imin, imax)
{
	if(imin && i < imin){this.rs.OnError(this.name + " : Value is out of range, " + i + " < " + imin);return 4;}
	if(imax && i > imax){this.rs.OnError(this.name + " : Value is out of range, " + i + " > " + imax);return 4;}
	return 0;
}

function valfloat(v)
{
	if(v != "")
	{if(this.currencySymbol != null){v = this.frdisp(v);}
		var i;
		if(this.Type == "r8") i = parseFloat(v);else i = Number(v);
		if(isNaN(i)){this.errNo = 3;this.rs.OnError(this.name + " : Invalid number");return null;}
		if((this.errNo = this.minmax(i, this.minval, this.maxval)) != 0) return null;
		v = this.todisp(i.toString());
	}
	this.errNo = 0;
	return v;
}

function dispfloat(n)
{
	var f; if(this.Type == "r8")f = parseFloat(n);else f = Number(n);
	if(this.decimals != null){var d = this.decimals;var p = Math.pow(10, d);f = (Math.round(f * p) / p);}
	n = f.toString();
	if(this.fixeddec != null && n.indexOf("e") == -1)
	{var j,i = n.indexOf(DecPoint);if(i == -1){n = n + DecPoint;i = 0;}else i = n.length - i - 1;
		for(j = i;j < this.fixeddec;j++) n = n + "0";
	}
	var c = this.currencySymbol;
	if(c != null){if(n.charAt(0) == '-') n = "(" + c + n.substring(1) + ")";else n = c + n;}
	return n;
}

function xmlfloat(n)
{
	var c = this.currencySymbol;if(c == null) return n;
	var s = n.indexOf("("),j = n.indexOf(c); 
	n = (j != -1) ? n.substring(j + c.length):n;
	if(s != -1)  n = "-" + n.substring(0, n.indexOf(")"));
	return n;
}

function xmldatetime(v)
{
	if(v == "") return v;
	var d = new Date(Date.parse(v));
	if(isNaN(d)){this.rs.OnError(this.name + " : Invalid date/time");return null;}
	var y = d.getFullYear();var m = (d.getMonth() + 1);var da = d.getDate();
	var h = d.getHours();var mi = d.getMinutes();var sec = d.getSeconds();var ms = d.getMilliseconds();var s = y.toString();
	if(m < 10) s = s + '0';
	s = s + m;
	if(da < 10) s = s + '0';
	s = s + da;
	if(h || mi || sec || ms)
	{
		s = s + 'T';
		if(h < 10) s = s + '0';
		s = s + h + ':';
		if(mi < 10) s = s + '0';
		s = s + mi + ':';
		if(sec < 10) s = s + '0';
		s = s + sec + ms;
	}
	return s;
}

function valdatetime(v)
{
	if(v != ""){var s = this.frdisp(v);if(!s) return s;v = this.todisp(s);}
	return v;
}
function valdate(v)
{
	return v;
}
function valtime(v)
{
	return v;
}


function DeltaChanges(ds)
{
	this.ds = ds;
	this.action = new Array();
	this.row = new Array();
	this.rowOrg = new Array();
	this.parents = new Array();
	this.rs = new Array();
	this.rem = RemFromLog;
	this.add = AddToLog;
	this.find = FindInLog;
	this.make = MakeDelta;
	this.fullpath = MakePath;
	this.reset = ResetLog;
}

function AddToLog(Act, pRow, pRowOrg, rs)
{
	var i,l = this.action.length;
	if(pRowOrg) for (i = 0;i < l;i++) if(this.row[i] == pRow) return;

	if(Act == szDel)
		for(i = 0;i < l;i++)
			if(this.row[i] == pRow)
			{
				if(this.action[i] == szNew){this.rem(i);return;}
				else{pRow = this.rowOrg[i];this.rem(i);l--;break;}
			}
	this.action[l] = Act;
	this.row[l] = pRow;
	this.rowOrg[l] = pRowOrg;
	this.parents[l] = GetParents(rs);
	this.rs[l] = rs;
}

function FindInLog(r)
{
	var i,l = this.action.length;
	for(i = 0;i < l;i++)if(this.row[i] == r) return i;
	return -1;
}

function ResetLog()
{
	var i,l = this.action.length;
	for(i = 0;i < l;i++){this.row[i].removeAttribute(szRowState);}
	this.action.length = 0;
}

function RemFromLog(j)
{
	var i,l = this.action.length;
	for(i = j;i < l - 1;i++)
	{
		this.action[i] = this.action[i + 1];
		this.row[i] = this.row[i + 1];
		this.rowOrg[i] = this.rowOrg[i + 1];
		this.parents[i] = this.parents[i + 1];
		this.rs[i] = this.rs[i + 1];
	}
	this.action.length = l - 1;
	this.row.length = l - 1;
	this.rowOrg.length = l - 1;
	this.parents.length = l - 1;
	this.rs.length = l - 1;
}

function MakeDelta()
{
	var ds = this.ds;
	var doc = ds.doc.createDocumentFragment();
	var e = ds.root.cloneNode(0);
	doc.appendChild(e);

	var mdata = ds.MetaData.cloneNode(1);
	var params = mdata.childNodes.item(1);
	params.setAttribute("DATASET_DELTA", "1");
	e.appendChild(mdata);

	var RD = ds.doc.createElement("ROWDATA");
	e.appendChild(RD);

	var l = this.action.length;
	var i;
 
	for(i = 0;i < l;i++)
	{
		var rs = this.rs[i];
		var pr = this.row[i].cloneNode(0);
		var po = this.rowOrg[i] != null ? this.rowOrg[i].cloneNode(1) :null;
		if(po)
		{
			po.setAttribute(szRowState, szOrg);
			var j,f,v1,v2,cnt = 0;
			for(j = 0;j < rs.Fields.Cnt;j++)
			{
				f = rs.Fields.Fieldx[j];
				v1 = f.get(po);
				v2 = f.get(pr);
				if (f.valcomp(v1, v2) == 0) f.put(pr, null);else cnt++;
			}
			if(cnt == 0){continue;}
		}
		pr.setAttribute(szRowState, this.action[i]);
		if(this.action[i] == szMod || this.action[i] == szNew || this.action[i] == szDel)
		{
			var j,le = this.row[i].childNodes.length;
			for(j = 0;j < le;j++) pr.appendChild(this.row[i].childNodes.item(j).cloneNode(0));
		}
		this.fullpath(RD, po, pr, i);
	}
	return e;
}

function MakePath(RD, po, pr, j)
{
	var pp = this.parents[j];
	if(pp == null){if(po) RD.appendChild(po);RD.appendChild(pr);return;}
	var i,rs = this.rs[j];
	for(i = pp.length - 1;i >= 0;i--){
		var pM = pp[i];
		pM = pM.cloneNode(0);
		pM.setAttribute(szRowState, szDetUpd);
		var FldLink = rs.doc.createElement(rs.linkFld);
		if(po){FldLink.appendChild(po);po = null;}
		FldLink.appendChild(pr);
		var k,l = pp[i].childNodes.length;
		for(k = 0;k < l;k++){if(pp[i].childNodes.item(k).tagName == rs.linkFld) pM.appendChild(FldLink);else pM.appendChild(pp[i].childNodes.item(k).cloneNode(0));}
		pr = pM;
		rs = rs.parent;
	}
	RD.appendChild(pr);
	return;
}

function GetParents(rs)
{
	var p = rs.parent;
	if(p == null)return null;
	var cnt = 1,i;
	var pa = new Array();
	while(p.parent != null){p = p.parent;cnt++;}
	p = rs.parent;
	for(i = 0;i < cnt;i++){pa[cnt - i - 1] = p.idx.row(p.pos);p = p.parent;}
	return pa;
}

function dsUndo(p)
{
	var s = this.RowState(p);//use attributes instead!!
	if(s == ""){this.notify(1);return;}
	if(s == "I"){this.deletex(p);}
	else if (s == "M")
	{
		var pRowOrg,pRow = this.idx.row(p);
		var c = this.DeltaChanges.find(pRow);
		if(c == -1) return;
		pRowOrg = this.DeltaChanges.rowOrg[c];
		var i,f,v;
		for(i = 0;i < this.Fields.Cnt;i++)
		{
			f = this.Fields.Fieldx[i];
			if(f.bAsAttr){v = pRowOrg.getAttribute(f.iname);f.put(pRow, v);}
		}
		pRow.removeAttribute(szRowState);
		this.idx.chg = 1;
		this.DeltaChanges.rem(c);
		this.notify(1); //?
	}
	else if(s == "D"){}
}

function DsForcePost()
{if(this.regobjs == null)return 0;
	var j ;
	for(j = 0;j < this.regobjs.length;j++)
	{
		var o = this.regobjs[j];
		if(o.post != null && o.post() != 0)return 1;
	}
	if(this.pDets == null)return 0;
	for(j = 0;j < this.pDets.length;j++){var det = this.pDets[j];if(det.forcepost() != 0)return 1;}
	return 0;
}

function idx(rs)
{
	this.rs = rs;
	this.chg = 0;
	this.add = idxadd;
	this.rem = idxrem;
	this.sort = idxsort;
	this.map = new Array();
	this.pos = idxpos;
	this.row = idxrow;
	this.fld = null;
	this.sort(null);
}
function idxadd(e)
{
	this.map[e] = this.map.length;
}
function idxpos(i)
{
	return this.map[i];
}
function idxsort(name)
{
	var i,s,cnt = this.rs.RowCnt;
	this.map.length = cnt;
	if(this.chg == 0 && name && name == this.fld){this.map.reverse();this.rs.first();this.rs.notify(2);return;}
	this.chg = 0; 
	for(i = 0;i < cnt;i++)this.map[i] = i;
	this.fld = name;
	if(name == null)return;
	var f = this.rs.Fields.Field[name];
	if(f == null)return;
	var sarray = new Array();
	this.fld = name;
	for(i = 0;i < cnt;i++){s = f.Value(i);s = s + "&" + i.toString();sarray[i] = s;}
	sarray.sort(f.valcomp);
	for(i = 0;i < cnt;i++){s = sarray[i].split("&");this.map[i] = parseInt(s[s.length - 1], 10);}
	this.rs.first();
	this.rs.notify(2);
}

function rsValue(p)
{
	var pos = p;
	if(isNaN(p))pos = this.rs.pos;
	var r = this.rs.idx.row(pos);
	if(r == null) return"";
	return this.get(r);
}

function getValue(r)
{
	if(this.bAsAttr)
	{
		var v = r.getAttribute(this.iname);
		if(v != null)
			return v;
		return "";
	}
	var p = findcnode(r, this.iname);
	if(p)
	{
		p = p.childNodes;
		if(p.length)
			return p.item(0).data;
	}
	return "";
}

function putValue(r, v)
{
	if(this.bAsAttr)
	{
		if(v != null)
		{
			r.setAttribute(this.iname, v);
		}
		else r.removeAttribute(this.iname);
		return;
	}
	if(v == null) v = "";
	var p = findcnode(r, this.iname);
	if(p)
	{
		if(p.childNodes.length == 0)
		{
			var T = this.rs.doc.createTextNode("");
			p.appendChild(T);
		}
		p.childNodes.item(0).data = v;
	}
}
