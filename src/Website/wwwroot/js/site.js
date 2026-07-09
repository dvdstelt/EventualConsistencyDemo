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

// The confirmation panels reveal a set of "async steps" purely as a visual;
// these handlers fill in the *real* order detail returned over SignalR once the
// back-end round-trip completes, proving the messaging actually ran.
connection.on('OrderedRegularTicket', function (ticket) {
    var tmpl = $('#regularticket').html();
    if (tmpl) {
        $('#orderDetails').html(tmpl.supplant(ticket));
    }
});

connection.on('OrderedLotteryTicket', function (ticket) {
    var tmpl = $('#lotteryticket').html();
    if (tmpl) {
        $('#raffleDetails').html(tmpl.supplant(ticket));
    }
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