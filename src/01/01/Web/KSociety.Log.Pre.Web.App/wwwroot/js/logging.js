"use strict";

const MESSAGE_LIMIT = 150;

var app = new Vue({
    el: '#eventLogApp',
    data: {
        filter: '',
        messages: [],
    },
    methods: {
        getRowColor(level) {
            let result = {
                WhiteRow: false,
                GreyRow: false,
                GreenRow: false,
                OrangeRow: false,
                RedRow: false,
                PurpleRow: false
            }
            switch (level) {
                case 0:
                    result.WhiteRow = true
                    break
                case 1:
                    result.GreyRow = true
                    break
                case 2:
                    result.GreenRow = true
                    break
                case 3:
                    result.OrangeRow = true
                    break
                case 4:
                    result.RedRow = true
                    break
                case 5:
                    result.PurpleRow = true
                    break
                default:
                    result.WhiteRow = true
                    break
            }
            return result
        },
        getFormattedDate(timestamp) {
            const date = new Date(timestamp)
            const options = {
                year: '2-digit',
                month: '2-digit',
                day: '2-digit',
                hour: '2-digit',
                minute: '2-digit',
                second: '2-digit'
            };

            return date.toLocaleDateString('it-IT', options) + `.${date.getMilliseconds()}`
        }
    },
    computed: {
        filteredMessages() {
            let currentFilter = this.filter
            let filterMessages = this.messages

            let levelFilter = /level:(\d+)/g.exec(currentFilter)
            if (levelFilter) {
                levelFilter = levelFilter
                let newFilter = currentFilter.slice(0, levelFilter.index) + currentFilter.slice(levelFilter.index + levelFilter[0].length)
                currentFilter = newFilter
                levelFilter = parseInt(levelFilter[1])
                filterMessages = _.filter(filterMessages, (message) => message.level === levelFilter)
            }

            filterMessages = _.filter(filterMessages, (message) => currentFilter !== '' ? message.message.includes(currentFilter) : true)
            return filterMessages;
        }
    }
})

var connection = new signalR.HubConnectionBuilder().withUrl("/loggingHub").build();

connection.on("ReceiveLog",
    function (logEvent) {
        app.messages.unshift(logEvent)
        if (app.messages.length > MESSAGE_LIMIT)
            app.messages.length.slice(0, MESSAGE_LIMIT)
    });

connection.logging = true;

connection.start().then(function () {
    //alert("Connection ok! ");
}).catch(function (err) {
    alert("Could not Connect! " + err.toString());
});