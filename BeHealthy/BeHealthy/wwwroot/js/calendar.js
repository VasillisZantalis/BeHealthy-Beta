document.addEventListener('DOMContentLoaded', function () {
    var calendarEl = document.getElementById('calendar');
    var calendar = new FullCalendar.Calendar(calendarEl, {
        initialView: 'dayGridMonth',
        headerToolbar: {
            start: 'prev,next today', // navigation buttons
            center: 'title',         // calendar title
            end: 'dayGridMonth,timeGridWeek,timeGridDay' // view options
        },
        events: [
            {
                title: 'Project Deadline',
                start: '2024-12-20',
                description: 'Final submission of the project',
                backgroundColor: '#ff5733',
                borderColor: '#c70039'
            },
            {
                title: 'Team Meeting',
                start: '2024-12-15T10:00:00',
                end: '2024-12-15T12:00:00',
                description: 'Discuss project progress',
                backgroundColor: '#33ff57',
                borderColor: '#33ff57'
            },
            {
                title: 'Holiday',
                start: '2024-12-25',
                allDay: true, // marks this as an all-day event
                description: 'Christmas holiday'
            },
            {
                title: 'Conference',
                start: '2024-12-28T09:00:00',
                end: '2024-12-28T17:00:00',
                description: 'Annual tech conference',
                backgroundColor: '#3375ff',
                borderColor: '#3375ff'
            }
        ],
        eventClick: function (info) {
            alert('Event: ' + info.event.title + '\nDescription: ' + info.event.extendedProps.description);
        }
    });

    calendar.render();
});

function initializeFullCalendar() {
    var calendarEl = document.getElementById('calendar');

    console.log("INITIALIZING CALENDAR");

    
}
