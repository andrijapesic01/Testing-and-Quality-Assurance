import { BoughtStock } from "./bought-stock";

export interface Portfolio
{
    id: number;
    ownerName: string;
    bankName: string;
    bankBalance: number;
    riskTolerance: number;
    investmentStrategy: string;
    boughtStocks: BoughtStock[];
}