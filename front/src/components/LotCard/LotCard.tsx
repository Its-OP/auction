import {JSXElementConstructor, useEffect, useState} from "react";
import {Card, Empty, Image, Typography} from "antd";
import {Lot} from "../../types/types.ts";
import {Link} from "react-router-dom";
import {currencyFormat} from "../../utils/currencyFormat.ts";
import {api} from "../../const/api.ts";
import {useHttp} from "../../hooks/shared/useHttp.ts";

export const LotCard:JSXElementConstructor<{lot: Lot }>=({lot})=>{
    const{imageUrl}= api
    const{request}= useHttp()

    const{minPrice,description,id,title}= lot

    const [image, setImage] = useState<string>()

    const getImg = async ()=>{
        const res = await request(imageUrl + id)

        if(res?.base64Body){
            setImage(res.base64Body)
        }
    }

    useEffect(() => {
        getImg()
    }, []);

    return <Card

        title={title}
        bodyStyle={{padding:"0px 0px 24px"}}
        extra={
            <Link to={`/lot/${id}`}>Детальніше</Link>
        }>

        {
            image ?  <Image   src={image}

                              width={"100%"} style={{maxHeight:300}}/>
                :<Empty/>
        }

        <div style={{padding:"0 24px 24px"}}>
            <div style={{display:"flex", alignItems:"center", justifyContent:"space-between",}}>
                <Typography.Title level={5} >Стартова ціна:{" "}{currencyFormat(minPrice)}</Typography.Title>
                <Typography.Text style={{marginTop:20}} type={"secondary"}>№ лоту:{" "}{id}</Typography.Text>
            </div>
            <Typography>{description}</Typography>
        </div>

    </Card>
}