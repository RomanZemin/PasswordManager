const baseUrl = 'https://localhost:5071/api/password';

const getAll = async () => {
    const response = await fetch(baseUrl);
    if (!response.ok) {
        throw new Error('Ошибка при получении паролей');
    }
    return response.json();
};

const createPassword = async (data) => {
    const response = await fetch(baseUrl, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
    });
    if (!response.ok) {
        throw new Error('Ошибка при создании пароля');
    }
    return response.json();
};

const deletePassword = async (id) => {
    const response = await fetch(`${baseUrl}/${id}`, {
        method: 'DELETE',
        headers: { 'Content-Type': 'application/json' }
    });
    if (!response.ok) {
        throw new Error('Ошибка при удалении пароля');
    }
    return response.text();
};

export default {
    getAll,
    createPassword,
    deletePassword
};
