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

async function initializeCalendar(events) {
    var calendarEl = document.getElementById('calendar');

    destroyCalendar();

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

            const calendarItem = {
                title: info.event.title,
                description: info.event.extendedProps.description,
                start: info.event.start,
                end: info.event.end,
                id: parseInt(info.event.id)
            };

            const calendarItemJson = JSON.stringify(calendarItem);

            DotNet.invokeMethodAsync("BeHealthy", "ConsoleEvent", calendarItemJson);
        },
    });

    calendar.render();
}
