import {Route, Routes, useNavigate} from "react-router-dom"
import {Main} from "./pages/Main/Main.tsx";
import {CurrentLot} from "./pages/Lot/Lot.tsx";
import {Auth} from "./pages/Auth/Auth.tsx";
import {useAuthContext} from "./context/authContext.tsx";
import {useEffect} from "react";

const NoMatch=({path}:{path:string})=>{
    const navigate = useNavigate()

    useEffect(() => {
        navigate(path)
    }, []);

    return<></>
}
export const Router =()=>{

    const {isAuth}= useAuthContext()

    return(
        <Routes>
            {
                isAuth ? <>
                    <Route path={"/"} element={<Main/>} />
                    <Route path={"/lot/:id"} element={<CurrentLot/>} />
                    <Route path={"/lot"} element={<CurrentLot/>} />
                    <Route path={"/*"} element={<NoMatch path={"/"}/>} />
                </> : <>
                    <Route path={"/auth"} element={<Auth/>} />
                    <Route path={"/*"} element={<NoMatch path={"/auth"}/>} />
                </>
            }

        </Routes>
    )
}