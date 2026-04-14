'use client';
import Link from 'next/link';

export default function Home() {
  return (
    <div className="min-h-screen flex">
      <div className="w-64 bg-gray-50 border-r border-gray-100 p-6 flex flex-col">
        <div className="mb-8">
          <h2 className="text-xl font-medium text-gray-800">pr</h2>
        </div>
        
        <nav className="flex-1 space-y-1">
          <Link href="/" className="block py-2 px-3 bg-gray-200 text-gray-800 rounded-md font-medium">
            Главная
          </Link>
          <Link href="/profile" className="block py-2 px-3 text-gray-700 hover:bg-gray-100 rounded-md transition-colors">
            Профиль
          </Link>
          <Link href="/health" className="block py-2 px-3 text-gray-700 hover:bg-gray-100 rounded-md transition-colors">
            Health
          </Link>
        </nav>

        <div className="pt-6 border-t border-gray-200">
          <Link href="/login" className="block py-2 px-3 text-gray-600 hover:text-gray-800 rounded-md text-center transition-colors">
            Вход
          </Link>
          <Link href="/register" className="block py-2 px-3 text-gray-600 hover:text-gray-800 rounded-md text-center transition-colors">
            Регистрация
          </Link>
        </div>
      </div>

      <div className="flex-1 bg-white flex items-center justify-center p-8">
        <div className="text-center max-w-lg">
          <h1 className="text-4xl font-light text-gray-800 mb-4">Добро пожаловать</h1>
         
        </div>
      </div>
    </div>
  );
}