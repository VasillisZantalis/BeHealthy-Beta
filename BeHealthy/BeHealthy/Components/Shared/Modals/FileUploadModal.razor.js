export function initializeDropzone(dropzoneElement, dotNetHelper) {
    if (!dropzoneElement) return;

    dropzoneElement.addEventListener('drop', async (e) => {
        e.preventDefault();
        e.stopPropagation();

        const files = e.dataTransfer.files;
        if (files.length > 0) {
            // This will trigger the file input change through .NET
            const fileInput = dropzoneElement.querySelector('input[type="file"]');
            if (fileInput) {
                // Create a new DataTransfer to assign files to input
                const dataTransfer = new DataTransfer();
                dataTransfer.items.add(files[0]);
                fileInput.files = dataTransfer.files;

                // Trigger change event
                const event = new Event('change', { bubbles: true });
                fileInput.dispatchEvent(event);
            }
        }
    });
}