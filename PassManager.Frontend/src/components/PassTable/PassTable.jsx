import { useState, useEffect, forwardRef, useImperativeHandle } from 'react';
import useInput from '../hooks/useInput';
import passwordService from '../services/passwordService';
import './PassTable.css';

const PassTable = forwardRef(({ refreshData, ...props }, ref) => {
    const input = useInput();
    const [loading, setLoading] = useState(false);
    const [passwords, setPasswords] = useState([]);
    const [visiblePasswords, setVisiblePasswords] = useState({});

    const fetchData = async () => {
        setLoading(true);
        try {
            const passwordList = await passwordService.getAll();
            setPasswords(passwordList);
        } catch (error) {
            console.error('Ошибка при получении паролей:', error);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchData();
    }, []);

    useImperativeHandle(ref, () => fetchData);

    const handlePasswordClick = (id) => {
        setVisiblePasswords(prev => ({
            ...prev,
            [id]: !prev[id]
        }));
    };

    const handleDelete = async (id) => {
        try {
            await passwordService.deletePassword(id);
            setPasswords(prevPasswords => prevPasswords.filter(password => password.id !== id));
        } catch (error) {
            alert(`Ошибка: ${error.message}`);
        }
    };

    return (
        <>
            {loading ? <p>Загрузка...</p> : (
                <>
                    <input type="text" id="search" placeholder='Поиск...' {...input} />
                    <table {...props}>
                        <thead>
                            <tr>
                                <th></th>
                                <th>Наименование</th>
                                <th>Пароль</th>
                                <th>Время обновления</th>
                            </tr>
                        </thead>
                        <tbody>
                            {passwords
                                .filter(password => password.name.toLowerCase().includes(input.value.toLowerCase()))
                                .map(password => (
                                    <tr key={password.id}>
                                        <td>
                                            <span className='delete-btn' onClick={() => handleDelete(password.id)}>
                                                &times;
                                            </span>
                                        </td>
                                        <td>{password.name}</td>
                                        <td className='password-cell' onClick={() => handlePasswordClick(password.id)}>
                                            {visiblePasswords[password.id] ? password.password : '********'}
                                        </td>
                                        <td>{new Date(password.createdAt).toLocaleString()}</td>
                                    </tr>
                                ))}
                        </tbody>
                    </table>
                </>
            )}
        </>
    );
});

export default PassTable;
