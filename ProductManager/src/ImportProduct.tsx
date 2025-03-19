import React, { useState } from "react";

interface ImportProductProps {
    onUploadSuccess: () => void;
}

const ImportProduct: React.FC<ImportProductProps> = ({ onUploadSuccess }) => {
    const [file, setFile] = useState<File | null>(null);
    const [uploading, setUploading] = useState(false);
    const [message, setMessage] = useState("");

    const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        if (event.target.files && event.target.files.length > 0) {
            setFile(event.target.files[0]);
        }
    };

    const handleUpload = async () => {
        if (!file) {
            alert("Please select a file first.");
            return;
        }

        const formData = new FormData();
        formData.append("file", file);

        setUploading(true);
        setMessage("");

        try {
            const response = await fetch("/api/products/upload-csv", {
                method: "POST",
                body: formData,
            });

            if (response.ok) {
                setMessage("CSV uploaded successfully!");
                onUploadSuccess(); // Notify parent component
            } else {
                const errorText = await response.text();
                setMessage(`Upload failed: ${errorText}`);
            }
        } catch (error) {
            setMessage("An error occurred while uploading.");
        } finally {
            setUploading(false);
        }
    };

    return (
        <div>
            <input type="file" accept=".csv" onChange={handleFileChange} className="form-control mb-2" />
            <button className="btn btn-success" onClick={handleUpload} disabled={uploading}>
                {uploading ? "Uploading..." : "Upload CSV"}
            </button>
            {message && <p className="mt-3">{message}</p>}
        </div>
    );
};

export default ImportProduct;
