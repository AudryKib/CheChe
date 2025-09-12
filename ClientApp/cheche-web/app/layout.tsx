import type { Metadata } from "next";

import "./globals.css";
import NavBar from "./navbar/NavBar";



export const metadata: Metadata = {
  title: "CheChe",
  description: " CheChe car Auctions web app",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      <body
        className=''
      >
        <NavBar/>
        <main className="container mx-auto px-5 pt-10">
        {children}
        </main>
      </body>
    </html>
  );
}
