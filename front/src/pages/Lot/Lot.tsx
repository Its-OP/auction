import {useParams} from "react-router";
import {useEffect, useState} from "react";
import {IBid, ImageTypes} from "../../types/types.ts";
import {Alert, Breadcrumb, Button, Card, Col, Empty, Form, Image, Input, List, Row, Typography} from "antd";
import {currencyFormat} from "../../utils/currencyFormat.ts";
import {Link} from "react-router-dom";
import {Helmet} from "react-helmet"
import {BASE_TITLE} from "../../const/const.ts";
import {useHttp} from "../../hooks/shared/useHttp.ts";
import {api} from "../../const/api.ts";
import {useLot} from "../../hooks/use/useLot.ts";
import {useAuthContext} from "../../context/authContext.tsx";

export const CurrentLot =()=>{

    const{imageUrl,bids:bidsUrl}= api
    const {id: lotId}= useParams()
    const {userName}= useAuthContext()

    const {request}= useHttp()

    const{lot, getLot, doBid,updateCurrentSum, lotLoading,closeLot}= useLot()
    const [thumbnail, setThumbnail] = useState<string>()
    const [gallery,setGallery] = useState<string[]>([])
    const [bids,setBids] = useState<IBid[]>([])

    const getBids =async ()=>{
    const res = await request(bidsUrl + lot?.id)

        setBids(res)
    }

    const getImg = async (id:number)=>{
        const res = await request(imageUrl + id)

           return  res?.base64Body

    }
    const getThumbnail =async ()=>{
        if(lot?.thumbnailId){
            const thumb = await getImg(lot.thumbnailId)
            setThumbnail(thumb)
        }
    }

    const getGallery =async ()=>{

        const promises = lot?.gallery.filter(img => img.type === ImageTypes.Gallery).map(img=>{

            return  getImg(img.id)
        }) ?? []

        const g:string[] =[]

      await  Promise.allSettled(promises).then((results) =>
            results.forEach((result) => {
                if(result.status === "fulfilled"){
                    g.push(result.value)
                }
            }),
        );

        setGallery(g)
    }

    useEffect(() => {
        if(lot){
           getThumbnail()
            getGallery()
            getBids()
        }
    }, [lot]);

    useEffect(() => {
      if(typeof lotId === "string"){
          getLot(+lotId)
      }
    }, []);

    const onFinish =async ({value}:any)=>{
      if(lotId){
          const res = await doBid({value,auctionId:+lotId})
          setBids((prev)=> ([...prev,res]))
          updateCurrentSum(res)
      }
    }
    const addMinBid = async ()=>{
        if(lotId){
            const res = await doBid({value: (lot?.winningBid?.value || lot?.minPrice || 0) + (lot?.minStakeValue ?? 0),auctionId:+lotId})
            setBids((prev)=> ([...prev,res]))
            updateCurrentSum(res)
        }
    }

const formater = new Intl.DateTimeFormat("uk-UA",{day:"numeric", month:"numeric",hour:"2-digit", minute:"2-digit",second:"2-digit"})

    if(!lot){
        return <Empty/>
    }
    return (<>
<Helmet>
    <title>{BASE_TITLE} | {lot?.title ?? "Лот"}</title>
</Helmet>
        <Breadcrumb
            items={[
                {
                    title: <Link to={"/"}>Лоти</Link>,
                },

                {
                    title: lot?.title,
                },
            ]}
        />
        <Typography.Title level={2} >{lot?.title}</Typography.Title>
        <Row gutter={[40,40]}>

            <Col xs={6}>
                <Row gutter={[20,20]}>
                    {
                        gallery.map((img,i)=>(<Col key={i} xs={12}><Image src={img}/></Col>))
                    }
                </Row>
            </Col>

            <Col xs={9}>
                <Image width={"100%"} src={thumbnail}/>
            </Col>

            <Col xs={9}>
                <Card
                    title={<Typography.Text style={{fontSize:"1.2rem"}} strong>Поточна ціна:{" "}{currencyFormat(lot?.winningBid?.value || lot.minPrice)}</Typography.Text>}
                     extra={<Typography.Text type={"secondary"}>Ставок:{" "}{bids?.length}</Typography.Text>}
                >
                <div style={{display:"flex", alignItems:"center", justifyContent:"space-between"}}>
                    <Typography.Text type={"secondary"}>Стартова ціна{" "}{currencyFormat(lot?.minPrice ?? 0)}</Typography.Text>
                    <Typography.Text type={"secondary"}>№ лоту: {" "}{lot?.id}</Typography.Text>
                </div>
                    <Typography style={{margin:"20px 0 30px"}}>{lot?.description}</Typography>

                    {
                        lot?.status === "Active"?          <Form onFinish={onFinish} layout={"inline"} style={{display:"flex"}}>
                            <div style={{display:"grid", gridTemplateColumns:"1fr 140px 120px"}}>
                                <Form.Item style={{flex:1}} required name={"value"}
                                           rules={[{ required: true, message: 'Будь ласка введіть суму ставки!' },
                                               //{min:lot?.winningBid.value + lot.minStakeValue ?? 0, message:"Сума не можу бути меншою ставки"}
                                           ]}>
                                    <Input  type={"number"} placeholder={"Сума ставки"}/>
                                </Form.Item>
                                <Form.Item >
                                    <Button loading={lotLoading} style={{width:"100%"}} htmlType={"submit"}  type={"primary"}>Зробити ставку</Button>
                                </Form.Item>
                                <Form.Item   >
                                    <Button loading={lotLoading} style={{width:"100%"}} onClick={addMinBid}  danger>+{" "}{currencyFormat(lot.minStakeValue)}</Button>
                                </Form.Item>
                            </div>
                        </Form>
                            :<Alert message={"Данний лот закритий"} type={"error"} />
                    }
                    {
                        lot.status === "Active" && lot.hostUsername === userName && <div style={{marginTop:30}}>
                        <Button style={{flex: 1}} type={"primary"} danger onClick={()=>closeLot(lot?.id)}>Закрити лот</Button>
                        </div>
                    }
                </Card>
                <List
                    style={{marginTop:30}}
                    header={<Typography.Title level={5}>Ставки</Typography.Title>}
                    bordered
                    dataSource={bids}
                    renderItem={(item:IBid) => (
                        <List.Item style={{display:"flex",alignItems:"center", gap:10}}>
                            <Typography.Text style={{flex:1}}>{item.username}</Typography.Text>
                            <Typography.Text >{currencyFormat(item.value)}</Typography.Text>
                            <Typography.Text >{
                                formater.format(new Date(item.timestamp))
                                }</Typography.Text>
                        </List.Item>
                    )}
                />
            </Col>


        </Row>
    </>)
}