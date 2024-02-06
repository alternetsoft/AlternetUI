export default {
    defaultTheme: 'light',

    iconLinks: [
        {
            icon: 'github',
            href: 'https://github.com/dotnet/docfx',
            title: 'GitHub'
        },
        {
            icon: 'twitter',
            href: 'https://twitter.com',
            title: 'Twitter'
        }
    ],
    start: () => {
        // Startup script goes here

        localStorage.setItem('theme', 'light');

        HTMLInputElement.prototype.realAddEventListener = HTMLInputElement.prototype.addEventListener;

        HTMLInputElement.prototype.addEventListener = function (a, b, c) {
            // this.realAddEventListener(a,b,c); 

            if (!this.eventListenerList) this.eventListenerList = {};
            if (!this.eventListenerList[a]) this.eventListenerList[a] = [];
            this.eventListenerList[a].push(b);

            //var a = this.lastListenerInfo[this.lastListenerInfo.length - 1];
            console.log("addEventListener " + a + " " + b);
        };

        // https://stackoverflow.com/questions/446892/how-to-find-event-listeners-on-a-dom-node-in-javascript-or-in-debugging
        /*(function() {
          Element.prototype._addEventListener = Element.prototype.addEventListener;
          Element.prototype.addEventListener = function(a,b,c) {
            this._addEventListener(a,b,c);
            if(!this.eventListenerList) this.eventListenerList = {};
            if(!this.eventListenerList[a]) this.eventListenerList[a] = [];
            this.eventListenerList[a].push(b);
          };
        })();*/


        window.addEventListener("beforeunload", function (event) {
            //your code goes here on location change 
            console.log("beforeunload " + window.location.href);
        });

        //window.addEventListener("load", (event) => {

            const onInterval = () => {

                if (!window.docfx)
                    return;
                if (!window.docfx.ready)
                    return;

                console.log("DocFx loaded");
                clearInterval(window._timerId);

                const elem = document.getElementById("search");

                if (elem) {
                    console.log("keyDown added");

                    elem.addEventListener('keydown', event => {
                        if(event.keyCode == 13 || event.key == "Enter")
                        {
                            console.log("User pressed key: " + event.key);

                            let elem2 = document.getElementById("search-query");
                            let eventList = elem2.eventListenerList;

                            if (eventList)
                            {
                                //console.log("has eventListenerList");
                                let evt = eventList["input"];
                                //console.log("evt");

                                if (evt && evt.length > 0)
                                {
                                    //console.log("has evt");
                                    let mth = evt[0];
                                    //console.log("has mth");
                                    if (mth)
                                    {
                                        //console.log("mth called");
                                        mth();
                                    }
                                }
                            }

                            event.preventDefault();
                            return false;
                        }
                    });
                }

            };

            window._timerId = setInterval(onInterval, 100);


        //});


    },
}

