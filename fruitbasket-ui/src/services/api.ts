import axios from "axios";
import { FruitItem, MandDetail } from "../types/Fruit";

export const addFruit = async (soort: string, data: any): Promise<void> => {
  await axios.put(`/fruit/${soort}`, data);
};

export const deleteMand = async (): Promise<void> => {
  await axios.delete(`/mand`);
};

export const deleteOldestFruit = async (soort: string): Promise<void> => {
  await axios.delete(`/fruit/${soort}`);
};

export const getOldestFruit = async (soort: string): Promise<FruitItem> => {
  const res = await axios.get(`/fruit/${soort}`);
  return res.data;
};

export const getMandDetail = async (): Promise<MandDetail> => {
  const res = await axios.get("/mand/detail");
  return res.data;
};