import {useLotApi} from "../api/useLotApi.ts";
import {useState} from "react";
import {IBid, Lot, Lots} from "../../types/types.ts";
import {HTTP_METHOD, useHttp} from "../shared/useHttp.ts";
import {api} from "../../const/api.ts";

export const useLot=()=>{
const {bids,auctions}= api
    const{lotApiLoading,fetchLots,fetchLot}= useLotApi()
    const{request,loading} = useHttp()

    const [lots,setLots] = useState<Lots>([])
    const [lot, setLot] = useState<Lot>()

   const getLots = async ()=>{
        const res =await fetchLots()

       setLots(res);
   }
    const getLot = async (id:number)=>{
        const res =await fetchLot(id)

        setLot(res);
    }

    const doBid =async (body:{
        "auctionId": number,
        "value": number
    }):Promise<IBid>=>{

        const res:IBid = await request(bids, HTTP_METHOD.POST, body)

        return res
    }

    const updateCurrentSum =(bid:IBid)=>{
        setLot(prev=>{
           if(prev){
               prev.winningBid = bid
           }
            return prev
        })
    }

    const closeLot = async(id:number)=>{
         const res = await request(`${auctions}close/${id}`, HTTP_METHOD.POST)
    }

    return{lotApiLoading,lots,lot,getLots, getLot,doBid,updateCurrentSum,lotLoading:loading,closeLot}
}