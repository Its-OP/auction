import {HTTP_METHOD, useHttp} from "../shared/useHttp.ts";
import {Lot, Lots} from "../../types/types.ts";
import {api} from "../../const/api.ts";

export const useLotApi =()=>{
const{auctions,auctionsforSearch}= api
    const {loading, request}= useHttp()


    const fetchLots = async(params?:string):Promise<Lots>=>{
                const res = await request(auctionsforSearch + (params ?? ""))

            return res
    }

    const fetchLot = async(id:number):Promise<Lot>=>{
                const res = await request(auctions + id)

            return res
    }

    const createLot = async (lot: any) => {
        const res = await request(auctions, HTTP_METHOD.POST, lot);

        return res
    }


    const editLot = async(lot:any)=>{
        await request(`${auctions}update/${lot.id}`, HTTP_METHOD.PUT, lot)

    }

    return{
        lotApiLoading:loading,
        fetchLots,
        createLot,
        fetchLot,
        editLot
    }

}