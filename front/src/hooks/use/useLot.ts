import {useLotApi} from "../api/useLotApi.ts";
import {useState} from "react";
import {IBid, Lot, Lots} from "../../types/types.ts";
import {HTTP_METHOD, useHttp} from "../shared/useHttp.ts";
import {api} from "../../const/api.ts";
import {useNavigate} from "react-router-dom";

export const useLot=()=>{

    const navigate = useNavigate()
    const {bids,auctions}= api
    const{lotApiLoading,fetchLots,fetchLot}= useLotApi()
    const{request,loading} = useHttp()

    const [lots,setLots] = useState<Lots>([])
    const [lot, setLot] = useState<Lot>()

   const getLots = async (params?:string)=>{
        const res =await fetchLots(params)

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
          await request(`${auctions}close/${id}`, HTTP_METHOD.POST)

        setLot(prev=>{

            if(prev){
                prev.status = "Closed"
            }
            return prev
        })
    }

    const editLot = async(lot:any)=>{
          await request(`${auctions}update/${lot.id}`, HTTP_METHOD.POST, lot)
        navigate("lot/" +lot.id)
       // setLot(lot)
    }

    return{lotApiLoading,lots,lot,getLots, getLot,doBid,updateCurrentSum,lotLoading:loading,closeLot,editLot}
}