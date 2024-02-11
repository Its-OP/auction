import {useEffect, useState} from "react";
import {HTTP_METHOD, useHttp} from "../shared/useHttp.ts";
import {api} from "../../const/api.ts";
import { useJwt } from "react-jwt";
import {useCookies} from "react-cookie";

export type authReqType={username: string, password: string}
export const useAuth =()=>{

    const {signInUrl,signUpUrl}= api
    const[cookies,setCookies,removeCookies]= useCookies(["token"])

    const {request,loading}= useHttp()
    const { decodedToken, isExpired,reEvaluateToken  } = useJwt(cookies.token);
    const [isAuth,setIsAuth]= useState<boolean>(false)
    const [userName,setUserName]= useState<string>("")



    const signIn = async(body:authReqType)=>{
        const response = await request(signInUrl, HTTP_METHOD.POST, body);

        if (!response.err) {
            setCookies("token",response.token )
            setIsAuth(true);

        }
    }
    useEffect(() => {
        reEvaluateToken(cookies.token)
    }, [cookies]);

    useEffect(() => {
       if(decodedToken){
           // @ts-ignore
           setUserName(decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"])
           setIsAuth(true)
       }else{
           setIsAuth(false)
       }
    }, [decodedToken]);

    useEffect(() => {
        if(isExpired){
            removeCookies('token')

        }
    }, [isExpired]);

    const signUp = async(body:authReqType)=>{
        const response = await request(signUpUrl, HTTP_METHOD.POST, body);
        if (!response.err) {
            setIsAuth(true);

        }
    }
    return{isAuth,userName, loading,signIn, signUp}
}