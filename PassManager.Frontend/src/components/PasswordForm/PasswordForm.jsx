import { useState } from 'react';
import passwordService from './../services/passwordService';
import './PasswordForm.css';

export default function PasswordForm({ closeModal, refreshData }) {
    const [name, setName] = useState('');
    const [password, setPassword] = useState('');
    const [type, setType] = useState('site');
    const [showPassword, setShowPassword] = useState(false);

    const handleSubmit = async (event) => {
        event.preventDefault();

        if (type === 'email' && !validateEmail(name)) {
            alert('Неверный формат электронной почты');
            return;
        }

        try {
            await passwordService.createPassword({ name, password, type, createdAt: new Date().toISOString() });
            setName(''); 
            setPassword('');
            setType('site');
            closeModal();
        } catch (error) {
            alert(`Ошибка: ${error.message}`);
        }
    };

    function validateEmail(email) {
        const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return re.test(email);
    }

    return (
        <form onSubmit={handleSubmit}>
            <span className="close-btn" onClick={() => setModal(false)}>&times;</span>
            <h2>Добавить новый пароль</h2>
            <label htmlFor="name">Наименование:</label>
            <input
                type="text"
                id="name"
                name="name"
                value={name}
                onChange={(event) => setName(event.target.value)}
                required
            />

            <label htmlFor="password">Пароль:</label>
            <div className="password-field">
                <input
                    type={showPassword ? 'text' : 'password'}
                    id="password"
                    name="password"
                    value={password}
                    onChange={(event) => setPassword(event.target.value)}
                    required
                    minLength="8"
                />
                <button
                    type="button"
                    onClick={() => setShowPassword(prev => !prev)}
                    className="toggle-password-btn"
                >
                    {showPassword ? 'Скрыть' : 'Показать'}
                </button>
            </div>

            <label>Тип записи:</label>
            <ul>
                    <input
                        type="radio"
                        id="typeSite"
                        name="type"
                        value="site"
                        checked={type === 'site'}
                        onChange={() => setType('site')}
                    />
                    <label htmlFor="typeSite">Сайт</label>
                    </ul><ul>
                    <input
                        type="radio"
                        id="typeEmail"
                        name="type"
                        value="email"
                        checked={type === 'email'}
                        onChange={() => setType('email')}
                    />
                    <label htmlFor="typeEmail">Электронная почта</label>
            </ul>

            <button type="submit">Сохранить</button>
            <button type="button" onClick={closeModal}>Отмена</button>
        </form>
    );
}
