import type { Metadata } from "next";

import "./globals.css";



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
        {children}
      </body>
    </html>
  );
}
