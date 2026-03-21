'use client';
import { useState, useEffect } from 'react';
import api from '@/lib/api';
import { AuthRes } from '@/types/auth';
import { useRouter } from 'next/navigation';
export default function LoginPage() {
     const router = useRouter()
    const [status, setStatus] = useState("");
;

   const handleLogout = () => {

        router.push('/');
    };
    useEffect(() => {
        const getHealth = async () => {
            try {
            const res = await api.get('/auth/health');
            if (res.data.status === 'ok') {
                setStatus("Online");
            }
            } catch {
                setStatus("Ofline");
            } 
        }
        getHealth();
}, [])
        
   

    return (
      <div className="flex items-center justify-center min-h-screen bg-gray-100">
    <div className="bg-white p-8 rounded-lg shadow-md border border-gray-200 text-center w-64">
      
      <h2 className="text-gray-400 text-xs uppercase font-bold mb-2">Сервер</h2>
      
      <div className={`text-2xl font-black ${
        status === "Online" ? "text-green-500" : "text-red-500"
      }`}>
        {status}
      </div>

    </div>
     <div className="absolute bottom-8 left-1/2 -translate-x-1/2">
                <button 
                    className="flex flex-col items-center gap-1 group"
                    onClick={handleLogout}
                >
                    <span className="text-red-400 group-hover:text-red-600 transition-colors text-sm font-bold uppercase tracking-wider">
                        Выйти
                    </span>
                    <div className="h-0.5 w-8 bg-red-200 group-hover:w-12 group-hover:bg-red-500 transition-all"></div>
                </button>
            </div>
  </div>
    );
 };