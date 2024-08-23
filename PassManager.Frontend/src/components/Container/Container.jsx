import { useState, useRef } from 'react';
import PassTable from './../PassTable/PassTable';
import Button from './../Button/Button';
import Modal from './../Modal/Modal';
import PasswordForm from './../PasswordForm/PasswordForm';
import './Container.css';

export default function Container() {
    const [modalOpen, setModalOpen] = useState(false);
    const fetchTableRef = useRef(null);

    const handleModalClose = () => {
        setModalOpen(false);
        if (fetchTableRef.current) {
            fetchTableRef.current();
        }
    };

    const handleModalOpen = () => {
        setModalOpen(true);
    };

    return (
        <div className="container">
            <h1>Менеджер паролей на React</h1>
            <PassTable ref={fetchTableRef} />
            <Button onClick={handleModalOpen} id='addRecordBtn'>Добавить новый</Button>
            <Modal open={modalOpen} onClose={handleModalClose}>
                <PasswordForm closeModal={handleModalClose} fetchTable={fetchTableRef} />
            </Modal>
        </div>
    );
}
