import "./App.css";
import AddFruitForm from "./components/AddFruitForm";
import BasketOverview from "./components/BasketOverview";
import { useRef } from "react";

function App() {
  const refreshRef = useRef<() => void>(() => {});

  return (
    <div className="container">
      <h1>🍇 Fruitmand App</h1>
      <AddFruitForm onFruitAdded={() => refreshRef.current()} />
      <hr />
      <BasketOverview setRefreshFunction={(fn) => (refreshRef.current = fn)} />
    </div>
  );
}

export default App;