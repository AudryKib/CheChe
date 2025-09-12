'use client'

import React from 'react'
import Countdown, { zeroPad } from 'react-countdown';

type CountDownTimerProps = {   
    auctionEnd: string;
}
// Renderer callback with condition
const renderer = ({ days, hours, minutes, seconds, completed }: 
    {days: number, hours:number, minutes: number, seconds: number, completed: boolean}) => {

        return (
            <div className={`border-2 
            border-white 
            text-white py-1 px-2 
            rounded-lg flex 
            items-center 
            justify-center
            ${completed ? 'bg-red-600' : (days <= 0 && hours < 10) ? 'bg-amber-600' : 'bg-green-600'}`}>
                {completed ? (
                  <span className='text-white'>Auction Ended</span>
                ):(
                <span suppressHydrationWarning={false} className='text-white'>{zeroPad(days)}:{ zeroPad(hours)}:{ zeroPad(minutes)}:{zeroPad(seconds)}</span>
                )}
              
            </div>
        )
  };

export default function CountDownTimer( { auctionEnd }: CountDownTimerProps) {
  if (!auctionEnd || isNaN(new Date(auctionEnd).getTime())) {
    return <div>Invalid auction end date</div>;
  }
  return (
    <div>
        <Countdown date={auctionEnd} renderer={renderer} />
    </div>
  )
}
