export default {
    defaultTheme: 'light',

    start: () => {
        localStorage.setItem('theme', 'light');

        // https://stackoverflow.com/questions/446892/how-to-find-event-listeners-on-a-dom-node-in-javascript-or-in-debugging
        HTMLInputElement.prototype.realAddEventListener = HTMLInputElement.prototype.addEventListener;
        HTMLInputElement.prototype.addEventListener = function (a, b, c) {
            // this.realAddEventListener(a,b,c); 

            if (!this.eventListenerList) this.eventListenerList = {};
            if (!this.eventListenerList[a]) this.eventListenerList[a] = [];
            this.eventListenerList[a].push(b);
            // console.log("addEventListener " + a + " " + b);
        };

        //==================================

        var oldReplaceState = history.replaceState;

        history.replaceState = (state, unused, url) => {
            const test = location.protocol + "//" + location.host + location.pathname + "?tabs=";

            if (url && url.startsWith(test)) {
                return;
            }

            oldReplaceState(state, unused, url);
        }

        //==================================

        const callEvent = (event, elem) => {
            let eventList = elem.eventListenerList;

            if (eventList) {
                let evt = eventList["input"];

                if (evt && evt.length > 0) {
                    let mth = evt[0];
                    if (mth) {

                        if (typeof mth === 'function') {
                            mth(event);
                        }
                        else
                            mth.handleEvent(event);
                    }
                }
            }
        }

        //==================================

        const addKeyDown = (formElem, elem) => {

            formElem.addEventListener('input', function (event) {
                const tocFilter = event.target.value.trim();
                const isEmpty = !tocFilter || tocFilter == '';
                if (isEmpty) {
                    callEvent(event, event.target);
                }
            });

            if (formElem) {

                formElem.addEventListener('keydown', event => {

                    const isEnter = event.keyCode == 13 || event.key == "Enter";
                    const isEscape = event.keyCode == 27 || event.key == "Escape";

                    // console.log(event.key + ' ' + event.keyCode);

                    if (isEnter) {
                        event.preventDefault();
                        callEvent(event, elem);
                        return false;
                    }
                    else if (isEscape) {
                        event.target.value = '';
                        event.preventDefault();
                        callEvent(event, elem);
                        return false;
                    }

                });
            }
        }

        //==================================

        const onInterval = () => {

            if (!window.docfx)
                return;
            if (!window.docfx.ready)
                return;

            clearInterval(window._timerId);

            let elem = document.getElementById("search");
            let elem2 = document.getElementById("search-query");
            addKeyDown(elem, elem2);

            var y = document.getElementsByClassName('filter');

            if (!y || y.length < 1)
                return;
            elem = y[0];

            y = elem.getElementsByClassName('form-control');
            if (!y || y.length < 1)
                return;
            elem2 = y[0];

            addKeyDown(elem, elem2);
        };

        window._timerId = setInterval(onInterval, 100);

        //==================================
    },
}
