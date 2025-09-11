'use server';

//import { fetchWrapper } from "../lib/fetchWrapper";
import { PagedResult, Auction }  from "../types";
//import { FieldValues } from "react-hook-form";

export async function getData(query: string): Promise<PagedResult<Auction>> {
    console.log('getData called with query:', query);
    const res = await fetch(`http://localhost:6001/search${query}`);

    if (!res.ok) {
        throw new Error(`Error fetching data: ${res.statusText}`);
    }

    return res.json() as Promise<PagedResult<Auction>>;
  //  return fetchWrapper.get(`search${query}`);

    
}