"use strict";

// Crockford's supplant method (poor man's templating)
if (!String.prototype.supplant) {
    String.prototype.supplant = function (o) {
        return this.replace(/{([^{}]*)}/g,
            function (a, b) {
                var r = o[b];
                return typeof r === 'string' || typeof r === 'number' ? r : a;
            }
        );
    };
}

let connection = new signalR.HubConnectionBuilder()
    .withUrl("/ticketHub")
    .build();

connection.start().catch(function (err) {
    return console.error(err.toString());
});

connection.on('OrderedRegularTicket', function (ticket) {
    var template = $('#regularticket').html().supplant(ticket);
    $('.overlay-content .ordering-details').html(template);
});

connection.on('OrderedLotteryTicket', function (ticket) {
    console.log('OrderedLotteryTicket');
    var template = $('#lotteryticket').html().supplant(ticket);
    $('.overlay-content .ordering-details').html(template);
});

function registerMovieTicket(formValues)
{
    var ticket = {
        MovieId: formValues["movieId"],
        TheaterId: formValues["theatersContext_group"],
        Time: formValues["times_group"],
        NumberOfTickets: parseInt(formValues["numberOfTickets"])
    };

    connection.invoke("SubmitOrder", ticket)
        .catch(function (err) {
            return console.error(err.toString());
        });
   
}