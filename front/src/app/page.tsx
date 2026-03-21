"use client"
import Link from 'next/link';

export default function Home() {
  return (
   <div className="flex flex-col flex-1 items-center justify-center bg-zinc-50 font-sans dark:bg-black">
    <div className='flex flex-col gap-2 w-fit'>
      <Link href="login">Login</Link>
      <Link href="register">Register</Link>
      <Link href="health">Health</Link>
      <Link href="profile">Profile</Link>
    </div>
   </div>
  );
}
