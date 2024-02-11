import {createContext, Dispatch, FC, ReactNode, SetStateAction, useContext, useState} from "react";
import {Lot} from "../types/types.ts";

type lotForEditType={
    lotForEdit:Lot | null,
    setLotForEdit:Dispatch<SetStateAction<Lot|null>>
}

const ChangeContext = createContext<lotForEditType>({lotForEdit:null,setLotForEdit:()=>{}})
export const ChangeLotContextProvider:FC<{children:ReactNode}> =({children})=>{

    const [lotForEdit, setLotForEdit] = useState<Lot | null>(null)



    return(<ChangeContext.Provider value={{lotForEdit,setLotForEdit}}>
        {children}
    </ChangeContext.Provider>)
}

export const useLotChangeContext =()=> useContext(ChangeContext)