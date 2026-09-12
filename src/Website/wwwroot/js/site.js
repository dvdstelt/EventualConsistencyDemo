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

// Kept so an order can be submitted the moment the page is interacted with,
// even if the connection handshake hasn't finished yet.
let connectionReady = connection.start().catch(function (err) {
    console.error(err.toString());
    throw err;
});

// The regular-ticket reply is the *only* proof the back-end actually processed the
// order, so the booking flow claims it and decides what the visitor gets to see
// (see film-booking.js). Without a listener the reply is simply dropped.
let regularTicketHandler = null;

function onRegularTicket(handler) {
    regularTicketHandler = handler;
}

connection.on('OrderedRegularTicket', function (ticket) {
    if (regularTicketHandler) {
        regularTicketHandler(ticket);
    }
});

// The raffle confirmation is composed by the website itself (MovieTickets), so it
// arrives whether or not the back-end endpoint is running.
connection.on('OrderedLotteryTicket', function (ticket) {
    var tmpl = $('#lotteryticket').html();
    if (tmpl) {
        $('#raffleDetails').html(tmpl.supplant(ticket));
    }
});

// Returns a promise so callers can tell "the website accepted the order" from
// "the website itself is unreachable". A resolved promise only means the command
// is on the queue; it says nothing about the back-end having handled it.
function registerMovieTicket(formValues)
{
    var ticket = {
        MovieId: formValues["movieId"],
        TheaterId: formValues["theatersContext_group"],
        Time: formValues["times_group"],
        NumberOfTickets: parseInt(formValues["numberOfTickets"])
    };

    return connectionReady.then(function () {
        return connection.invoke("SubmitOrder", ticket);
    });
}
