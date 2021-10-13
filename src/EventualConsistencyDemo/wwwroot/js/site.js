"use strict";

let connection = new signalR.HubConnectionBuilder()
    .withUrl("/ticketHub")
    .build();

connection.start().catch(function (err) {
    return console.error(err.toString());
});

connection.on('OrderSubmission', function (message) {
    console.log('OrderSubmission', message);
    $('.overlay-content .ordering-details').html(message);
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