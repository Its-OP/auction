import {App, Layout} from "antd";
import {BrowserRouter} from "react-router-dom";
import {Header} from "../Header/Header.tsx";
import {Content} from "antd/es/layout/layout";
import {Router} from "../../Router.tsx";
import {CreateLot} from "../CreateLot/CreateLot.tsx";
import {useEffect, useState} from "react";
import {useAuthContext} from "../../context/authContext.tsx";

export const MainLayout =()=>{
    const [open, setOpen] = useState(false);
const {isAuth}= useAuthContext()

    const showDrawerCreateLot = () => {
        setOpen(true);
    };

    const onClose = () => {
        setOpen(false);
    };


    useEffect(() => {
        if(!isAuth){
            onClose()
        }
    }, [isAuth]);
    return( <App message={{duration:5, maxCount:3}} >
        <BrowserRouter>
            <Layout style={{height:"100vh"}}>
                <Header showDrawerCreateLot={showDrawerCreateLot} />

                <Content style={{maxWidth:1280,margin:"auto", marginTop: 40, width:"100%"}}>
                    <Router/>
                </Content>
                <CreateLot open={open} onClose={onClose} />

            </Layout>
        </BrowserRouter>
    </App>)
}