var calendar;

function populateCalendar(events) {
    if (calendar) {
        calendar.removeAllEvents();
        calendar.addEventSource(events);
    }
} 

function destroyCalendar() {
    if (calendar) {
        calendar.destroy();
        calendar = null;
    }
}

function initializeCalendar(dotNetRef, events) {
    var calendarEl = document.getElementById('calendar');

    destroyCalendar();

    calendar = new FullCalendar.Calendar(calendarEl, {
        initialView: 'dayGridMonth',
        locale: getLocale(),
        timeZone: 'local',
        headerToolbar: {
            start: 'prev,next today',
            center: 'title',
            end: 'dayGridMonth,timeGridWeek,timeGridDay,listWeek'
        },
        events: events,
        eventClick: function (info) {
            const calendarItem = {
                id: parseInt(info.event.id),
                title: info.event.title,
                description: info.event.extendedProps.description,
                start: info.event.start.toISOString(),
                end: info.event.end ? info.event.end.toISOString() : info.event.start.toISOString(),
                backgroundColor: info.event.backgroundColor,
                borderColor: info.event.borderColor,
                color: info.event.color
            };

            dotNetRef.invokeMethodAsync('OpenEventModal', calendarItem);
        },
    });

    calendar.render();
}
