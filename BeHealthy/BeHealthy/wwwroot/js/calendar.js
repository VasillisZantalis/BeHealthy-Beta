var calendar;

function populateCalendar(events) {
    if (calendar) {
        calendar.removeAllEvents();
        calendar.addEventSource(events);
    }
}

function initializeCalendar(events) {

    var calendarEl = document.getElementById('calendar');
    calendar = new FullCalendar.Calendar(calendarEl, {
        initialView: 'dayGridMonth',
        locale: getLocale(),
        headerToolbar: {
            start: 'prev,next today',
            center: 'title',
            end: 'dayGridMonth,timeGridWeek,timeGridDay,listWeek'
        },
        events: events,
        eventClick: function (info) {
            alert('Event: ' + info.event.title + '\nDescription: ' + info.event.extendedProps.description);
        },
    });

    calendar.render();
}
