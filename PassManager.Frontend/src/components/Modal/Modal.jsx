import React, { useRef, useEffect } from 'react';
import { createPortal } from 'react-dom';
import './Modal.css';

export default function Modal({ children, open, onClose }) {
    const dialogRef = useRef(null);

    useEffect(() => {
        if (open) {
            dialogRef.current.showModal();
        } else {
            dialogRef.current.close();
        }
    }, [open]);

    return createPortal(
        <dialog ref={dialogRef} className="modal-content" onClose={onClose}>
            {children}
        </dialog>,
        document.getElementById('modal')
    );
}
