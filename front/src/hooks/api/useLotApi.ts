import {HTTP_METHOD, useHttp} from "../shared/useHttp.ts";
import {Lot, Lots} from "../../types/types.ts";
import {api} from "../../const/api.ts";

export const useLotApi =()=>{
const{auctions}= api
    const {loading, request}= useHttp()


    const fetchLots = async():Promise<Lots>=>{
                const res = await request(auctions)

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

    return{
        lotApiLoading:loading,
        fetchLots,
        createLot,
        fetchLot
    }

}