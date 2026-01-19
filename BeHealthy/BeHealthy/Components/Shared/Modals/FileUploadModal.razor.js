export function initializeDropzone(dropzoneElement, dotNetHelper) {
    if (!dropzoneElement) return;

    // Prevent default drag behavior on the dropzone
    dropzoneElement.addEventListener('dragover', (e) => {
        e.preventDefault();
        e.stopPropagation();
    });

    dropzoneElement.addEventListener('drop', async (e) => {
        e.preventDefault();
        e.stopPropagation();

        const files = e.dataTransfer.files;
        if (files.length > 0) {
            const file = files[0];
            
            // Read file as base64
            const reader = new FileReader();
            reader.onload = async function(event) {
                try {
                    const base64Data = event.target.result;
                    
                    // Call Blazor method to handle the dropped file
                    await dotNetHelper.invokeMethodAsync('HandleDroppedFile', 
                        file.name, 
                        file.type, 
                        file.size, 
                        base64Data
                    );
                } catch (error) {
                    console.error('Error processing dropped file:', error);
                }
            };
            
            reader.onerror = function(error) {
                console.error('Error reading file:', error);
            };
            
            // Read the file as Data URL (base64)
            reader.readAsDataURL(file);
        }
    });
}