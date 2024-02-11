import {Button, Layout, theme, Typography} from "antd"
import {Link} from "react-router-dom";
import React from "react";
import {useAuthContext} from "../../context/authContext.tsx";

const {Header:HeaderComponent} = Layout

export const Header:React.FC<{showDrawerCreateLot:()=> void}> =({showDrawerCreateLot})=>{
    const {
        token: { colorBgContainer },
    } = theme.useToken();

const {isAuth} = useAuthContext()

    if(!isAuth){
        return null
    }

    return(
        <HeaderComponent   style={{ display: 'flex', alignItems: 'center',gap:20, backgroundColor:colorBgContainer }}>

               <Link to={"/"} style={{flex:1}}><Typography.Link style={{fontSize:"1.5rem"}}   keyboard>Усі лоти</Typography.Link></Link>

                <Button type={"primary"} onClick={showDrawerCreateLot}>Створити Лот</Button>

        </HeaderComponent>
    )
}