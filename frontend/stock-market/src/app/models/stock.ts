
export interface Stock {
    id: number;
    symbol: string;
    company: string;
    currentPrice: number;
    priceChange: number;
    percentChange: number;
    logoURL: string;
}