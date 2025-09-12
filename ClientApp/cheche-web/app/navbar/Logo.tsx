    'use client'

    import { IoCarSport } from "react-icons/io5";
    import { useParamsStore } from '../hooks/useParamsStore';

    import React from 'react'
    
    export default function Logo() {

        const  reset = useParamsStore(state => state.reset);
      return (
        <div className="flex items-center gap-2 text-3xl font-bold text-red-500 cursor-pointer" onClick={reset}>
          <IoCarSport size={34} />
          <div className="flex flex-col leading-tight">
               <div>CheChe</div>
          <div className="text-sm text-gray-600 outline-amber-600
          ">Every day Deals!</div>
          </div>
       
        </div>
      )
    }
     