var calendar;

function populateCalendar(events) {
    if (calendar) {
        calendar.removeAllEvents();
        calendar.addEventSource(events);
    }

    console.log(events);
}

function initializeCalendar(events) {

    console.log(events);

    var calendarEl = document.getElementById('calendar');
    calendar = new FullCalendar.Calendar(calendarEl, {
        initialView: 'dayGridMonth',
        headerToolbar: {
            start: 'prev,next today',
            center: 'title',
            end: 'dayGridMonth,timeGridWeek,timeGridDay'
        },
        events: events, // Pass the events here during initialization
        eventClick: function (info) {
            alert('Event: ' + info.event.title + '\nDescription: ' + info.event.extendedProps.description);
        }
    });

    calendar.render();
}
