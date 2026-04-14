'use client';
import { useState } from 'react';
import api from '@/lib/api';
import { AuthRes } from '@/types/auth';
import { useRouter } from 'next/navigation';
import Link from 'next/link';

export default function RegisterPage() {
    const router = useRouter();
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');

    const handleRegister = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            const { data } = await api.post<AuthRes>('/auth/register', { email, password });
            localStorage.setItem('token', data.token);
            localStorage.setItem('refreshToken', data.refreshToken);
            router.push('/profile');
        } catch (err: any) {
            setError('Ошибка при регистрации');
        }
    };

    return (
        <div className="min-h-screen flex">
            <div className="w-64 bg-gray-50 border-r border-gray-100 p-6 flex flex-col">
                <div className="mb-8">
                    <h2 className="text-xl font-medium text-gray-800">pr</h2>
                </div>
                
                <nav className="flex-1 space-y-1">
                    <Link href="/" className="block py-2 px-3 text-gray-700 hover:bg-gray-100 rounded-md transition-colors">
                        Главная
                    </Link>
                </nav>

                <div className="pt-6 border-t border-gray-200">
                    <Link href="/login" className="block py-2 px-3 text-gray-600 hover:text-gray-800 rounded-md text-center transition-colors">
                        Вход
                    </Link>
                    <Link href="/register" className="block py-2 px-3 bg-gray-200 text-gray-800 rounded-md font-medium mb-2 text-center">
                        Регистрация
                    </Link>
                </div>
            </div>

            <div className="flex-1 bg-white flex items-center justify-center p-8">
                <div className="w-full max-w-sm">
                    <div className="mb-8">
                        <h1 className="text-2xl font-medium text-gray-800 mb-1">Регистрация</h1>
                        <p className="text-gray-500 text-sm">Создайте новый аккаунт</p>
                    </div>

                    <form onSubmit={handleRegister} className="space-y-4">
                        <div>
                            <label className="block text-sm text-gray-600 mb-1">Email</label>
                            <input 
                                type="email" 
                                value={email}
                                onChange={e => setEmail(e.target.value)}
                                className="w-full px-4 py-2.5 border border-gray-200 rounded-md text-gray-700 focus:outline-none focus:border-gray-400"
                                placeholder="email@example.com"
                            />
                        </div>

                        <div>
                            <label className="block text-sm text-gray-600 mb-1">Пароль</label>
                            <input 
                                type="password" 
                                value={password}
                                onChange={e => setPassword(e.target.value)}
                                className="w-full px-4 py-2.5 border border-gray-200 rounded-md text-gray-700 focus:outline-none focus:border-gray-400"
                                placeholder="••••••••"
                            />
                        </div>

                        {error && (
                            <p className="text-red-500 text-sm">{error}</p>
                        )}

                        <button 
                            type="submit"
                            className="w-full py-2.5 bg-gray-800 text-white rounded-md hover:bg-gray-700 transition-colors font-medium mt-2"
                        >
                            Зарегистрироваться
                        </button>
                    </form>

                    <p className="text-center text-gray-500 text-sm mt-6">
                        Уже есть аккаунт?{' '}
                        <Link href="/login" className="text-gray-800 hover:underline">
                            Войти
                        </Link>
                    </p>
                </div>
            </div>
        </div>
    );
}