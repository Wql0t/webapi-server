'use client';
import { useState } from 'react';
import api from '@/lib/api';
import { AuthRes } from '@/types/auth';
import { useRouter } from 'next/navigation';
export default function LoginPage() {
    const router = useRouter()
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');

    const handleLogin = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            const { data } = await api.post<AuthRes>('/auth/login', { email, password });
console.log("Пришло от сервера:", data);
            localStorage.setItem('token', data.token);
            localStorage.setItem('refreshToken', data.refreshToken);

           router.push('/profile');
        } catch (err: any) {
            alert("Ошибка")
        }
    };

    return (
        <div className="flex flex-col items-center justify-center min-h-screen bg-white text-black">
            <form onSubmit={handleLogin} className="border p-10 rounded shadow-lg flex flex-col gap-4 w-96">
                <h1 className="text-2xl font-bold">Вход</h1>
                <input 
                    type="email" 
                    placeholder="Email" 
                    className="border p-2 rounded" 
                    onChange={e => setEmail(e.target.value)} 
                />
                <input 
                    type="password" 
                    placeholder="Пароль" 
                    className="border p-2 rounded" 
                    onChange={e => setPassword(e.target.value)} 
                />
                <button type="submit" className="bg-blue-600 text-white p-2 rounded hover:bg-blue-700">
                    Войти
                </button>
            </form>
        </div>
    );
}