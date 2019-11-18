import React, {useEffect, useState} from 'react';
import uuid from 'react-uuid';
import PropTypes from 'prop-types';
import Input from './Input';
import axios from 'axios';
import Result from "./Result";

const InputTable = ({supplyCount, demandCount}) => {
    const [supplies, setSupplies] = useState([]);
    const [demands, setDemands] = useState([]);
    const [prices, setPrices] = useState([[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0],[0,0,0,0,0]]);
    const [result, setResult] = useState({});
    useEffect(() => {
        if(prices.length!==supplyCount||prices[0].length!==demandCount) {
            const res = [];
            for (let i = 0; i < supplyCount; i++) {
                const columns = [];
                for (let j = 0; j < demandCount; j++) {
                    columns.push(0);
                }
                res.push(columns);
            }
            setPrices(res);
            const rows=[];
            for (let i = 0; i < supplyCount; i++) {
                rows.push(0);
            }
            setSupplies(rows);
            const cols=[];
            for (let i = 0; i < demandCount; i++) {
                cols.push(0);
            }
            setDemands(cols);
        }
    }, [supplyCount,demandCount]);
    const handleArrayChange = (e, id_i,id_j) => {
        if (e.target.name === 'supplies') {
            const supl = supplies.slice(); // Make a copy of the emails first.
            supl[id_i] = +e.target.value; // Update it with the modified email.
            setSupplies(supl);
        } else if (e.target.name === 'demands') {
            const dem = demands.slice(); // Make a copy of the emails first.
            dem[id_i] = +e.target.value; // Update it with the modified email.
            setDemands(dem);
        }
        else {
            const pri = prices.slice();
            pri[id_i][id_j]= +e.target.value;
            setPrices(pri);
        }
    };
    const createRow = (count, count2, values, name) => {
        if (name === 'prices') {
            const rows = [];
            for (let i = 0; i < values.length; i++) {
                const row = [];
                for (let j = 0; j < values[0].length; j++) {
                    row.push(<td><Input
                        //key={uuid()}
                        value={prices[i][j]}
                        //value={values[i]}
                        onChange={(e) => handleArrayChange(e, i, j)}
                        name={name}
                    /></td>)
                }
                rows.push(<tr>{row}</tr>);
            }
            return rows
        } else {
            const row = [];
            for (let i = 0; i < values.length; i++) {
                row.push(<td><Input
                    //key={uuid()}
                    value={values[i]}
                    onChange={(e) => handleArrayChange(e, i,null)}
                    name={name}
                /></td>)
            }
            return row;
        }
    };
    const handleSave=()=>{
        const data ={
            "supply":supplies,
            "demmand":demands,
            "pricing":prices,
        };
        axios.post(`/api/api/solve`, data)
            .then(res => {
                console.log(res);
                console.log(res.data);
                setResult(res.data);
            }).catch(err=>console.log(err))
    };
    return (
        <>
            <table>
                <tbody>
                <tr>{createRow(supplyCount, null, supplies, 'supplies')}</tr>
                </tbody>
            </table>
            <table>
                <tbody>
                <tr>{createRow(demandCount, null, demands, 'demands')}</tr>
                </tbody>
            </table>
            <hr/>
            <table>
                <tbody>
                {createRow(supplyCount, demandCount, prices, 'prices')}
                </tbody>
            </table>
            <button onClick={handleSave}>Calculate</button>
            {result.base_plan&&<p>Base Plan:</p>}
            {result.base_plan&&<Result array={result.base_plan} value={result.base_cost}/>}
            {result.optimized_plan&&<p>Optimized Plan:</p>}
            {result.optimized_plan&&<Result array={result.optimized_plan} value={result.optimized_cost}/>}
        </>
    );
};

InputTable.propTypes = {};
InputTable.defaultProps = {};

export default InputTable;
