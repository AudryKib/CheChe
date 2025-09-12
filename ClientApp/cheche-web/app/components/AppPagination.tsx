'use client'

import { Pagination } from 'flowbite-react';
import React from 'react'

type AppPaginationProps = {
  currentPage: number;
  pageCount: number;
  pageChanged: (page: number) => void;  
}

export default function AppPagination( {currentPage, pageCount, pageChanged}: AppPaginationProps) {
 
  
 
    return (
    <Pagination
    currentPage={currentPage}
    onPageChange={ (e) => pageChanged(e)}
   totalPages={Math.max(1, pageCount || 1)}
    layout="pagination"
    showIcons
    previousLabel="Previous"
    className='text-blue-500 mb-5'
    />
  )
}
