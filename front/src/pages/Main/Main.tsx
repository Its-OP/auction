import {Alert, Col, Empty, Row, Spin} from "antd";
import {LotCard} from "../../components/LotCard/LotCard.tsx";
import {Helmet} from "react-helmet";
import {BASE_TITLE} from "../../const/const.ts";
import {useLot} from "../../hooks/use/useLot.ts";
import {useEffect} from "react";

export const Main =()=>{

const {lots,getLots, lotApiLoading}= useLot()

    useEffect(() => {
        getLots()
    }, []);

    return(<>
        <Helmet>
            <title>
                {BASE_TITLE}
            </title>
        </Helmet>
        <Row gutter={[30,30]}>
            {
                lotApiLoading ?
                    <Spin size={"large"}>
                        <Alert message={"Завантаження"} type={"info"}/>
                    </Spin>
                 : <>
                    {
                        lots.map(lot =>(<Col key={lot.id}  xs={8}><LotCard lot={lot} /></Col>))
                    }
                </>
            }
            {
                !lotApiLoading && lots.length === 0 ? <Empty />: null
            }

        </Row>
    </>)
}