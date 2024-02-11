export interface StakeValue {
    amount: number;
}

export interface Stake {
    amount: number;
    datetime: string;
    username: string;
}
export interface Lot {
	id: number;
	title: string;
	minPrice: number;
	minStakeValue: number;
	description: string;
	status: string;
	thumbnailId: number;
	gallery: LotGallery[];
	hostUsername: string;
	winningBid: LotWinningBid;
}
export interface LotGallery {
	id: number;
	type: string;
}

export interface IBid {
	auctionId: number;
	username: string;
	value: number;
	timestamp: string;
}
export interface LotWinningBid {
	auctionId: number;
	username: string;
	value: number;
	timestamp: string;
}


export interface Token {
    jwt: string;
}

export interface User {
    username: string;
    password: string;
}

// Additional types for auction endpoints
export interface Lots extends Array<Lot> {}

export enum ImageTypes{
    Thumbnail= "Thumbnail",
    Gallery = "Gallery"
}