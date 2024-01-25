import { Stock } from "./stock";

export interface BoughtStock {
    id: number;
    stock: Stock;
    buyingPrice: number;
    quantity: number;
}