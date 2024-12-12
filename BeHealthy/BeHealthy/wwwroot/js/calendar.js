function initializeFullCalendar() {
    var calendarEl = document.getElementById('calendar');

    var calendar = new FullCalendar.Calendar(calendarEl, {
        initialView: 'dayGridMonth',
        events: [
            { title: 'Meeting', start: '2024-12-15' },
            { title: 'Conference', start: '2024-12-20', end: '2024-12-22' }
        ]
    });

    calendar.render();
}
