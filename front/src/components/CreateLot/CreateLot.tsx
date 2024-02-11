import React from 'react';
import {Button, Form, GetProp, Input, message, Upload, type UploadFile, UploadProps,Drawer} from "antd";
import {useState} from "react";
import { LoadingOutlined, PlusOutlined } from '@ant-design/icons';
import {GalleryList} from "./FormComponents/GalleryList.tsx";
import {useLotApi} from "../../hooks/api/useLotApi.ts";
import {ImageTypes} from "../../types/types.ts";
import {useNavigate} from "react-router-dom";
import {useForm} from "antd/es/form/Form";



type FileType = Parameters<GetProp<UploadProps, 'beforeUpload'>>[0];

const getBase64 = (img: FileType, callback: (url: string) => void) => {
    const reader = new FileReader();
    reader.addEventListener('load', () => callback(reader.result as string));
    reader.readAsDataURL(img);
};
const dummyRequest = ({ onSuccess }: any) => {
    setTimeout(() => {
        onSuccess("ok");
    }, 0);
};
const beforeUpload = (file: FileType) => {
    const isJpgOrPng = file.type === 'image/jpeg' || file.type === 'image/png';
    if (!isJpgOrPng) {
        message.error('You can only upload JPG/PNG file!');
    }
    const isLt2M = file.size / 1024 / 1024 < 2;
    if (!isLt2M) {
        message.error('Image must smaller than 2MB!');
    }
    return isJpgOrPng && isLt2M;
};

export const CreateLot: React.FC<{open:boolean, onClose:()=>void}> = ({open,onClose}) => {
    const [form ]= useForm()
    const navigate = useNavigate()
    const {createLot, lotApiLoading} = useLotApi()

    const [loading, setLoading] = useState(false);
    const [thumbnailUrl, setThumbnailUrl] = useState<string>();

    const [galleryUrls, setGalleryUrls] = useState<UploadFile[]>([]);

    const clearValues = ()=>{
        form.resetFields()
        setThumbnailUrl("")
        setGalleryUrls([])
    }

    const handleChange: UploadProps['onChange'] = (info) => {
        if (info.file.status === 'uploading') {
            setLoading(true);
            return;
        }
        if (info.file.status === 'done') {
            // Get this url from response in real world.
            getBase64(info.file.originFileObj as FileType, (url) => {
                setLoading(false);
                setThumbnailUrl(url);
            });
        }
    };


    const onFinish = async (values:any)=>{
        const data ={
            ...values,
            minPrice : Number(values?.minPrice ?? 0),
            minStakeValue : Number(values?.minStakeValue ?? 0),
            images: [

                {metadata:{
                        type:ImageTypes.Thumbnail
                    },
                    base64Body:thumbnailUrl
                },
                ...galleryUrls.map((file)=>({
                    metadata:{type:ImageTypes.Gallery},
                    base64Body:file.thumbUrl
                }))
            ]
        }
        const res = await createLot(data)

        if(res?.id){
            onClose()
            navigate("/lot/" + res.id)
            clearValues()
        }
    }

    const closeHandler=()=>{
        clearValues()
        onClose()
    }
    const uploadButton = (
        <button style={{ border: 0, background: 'none' }} type="button">
            {loading ? <LoadingOutlined /> : <PlusOutlined />}
            <div style={{ marginTop: 8 }}>Upload</div>
        </button>
    );

    return (

            <Drawer title="Створити лот" onClose={closeHandler} open={open} width={480}>
                <div style={{ padding:"100px 30px"}}>

                    <Form form={form}  style={{width:"100%"}} layout={"vertical"} onFinish={onFinish}>
                        <Form.Item label={"Назва лоту"} name={"title"}>
                            <Input  />
                        </Form.Item>
                        <Form.Item label={"Опис"}  name={"description"}>
                            <Input.TextArea />
                        </Form.Item>
                        <Form.Item label={"Початкова ставка"}  name={"minPrice"}>
                            <Input type={"number"} />
                        </Form.Item>
                        <Form.Item label={"Мінімальна ставка"}  name={"minBidValue"}>
                            <Input type={"number"} />
                        </Form.Item>

                        <Form.Item label={"Головне зображення"} >
                            <Upload
                                customRequest={dummyRequest}
                                listType="picture-card"
                                className="avatar-uploader"
                                showUploadList={false}
                                beforeUpload={beforeUpload}
                                onChange={handleChange}
                            >
                                {thumbnailUrl ? <img src={thumbnailUrl} alt="avatar" style={{ width: '100%' }} /> : uploadButton}
                            </Upload>
                        </Form.Item>

                        <Form.Item  label={"Фотографії"}>
                            <GalleryList galleryUrls={galleryUrls} setGalleryUrls={setGalleryUrls}/>

                        </Form.Item>
                        <Form.Item>
                            <Button loading={lotApiLoading} htmlType={"submit"} type={"primary"}>Опублікувати</Button>
                        </Form.Item>
                    </Form>
                </div>
            </Drawer>

    );
};