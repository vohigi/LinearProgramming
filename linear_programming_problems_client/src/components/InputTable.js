import React, {useState} from 'react';
import PropTypes from 'prop-types';
import Input from './Input'

const InputTable = ({supplyCount, demandCount}) => {
    const [supplies, setSupplies] = useState([]);
    const [demands, setDemands] = useState([]);
    const [prices, setPrices] = useState([]);
    const handleArrayChange = (e, id) => {
        if (e.target.name === 'supplies') {
            const supl = supplies.slice(); // Make a copy of the emails first.
            supl[id] = +e.target.value; // Update it with the modified email.
            setSupplies(supl);
        } else if (e.target.name === 'demands') {
            const dem = demands.slice(); // Make a copy of the emails first.
            dem[id] = +e.target.value; // Update it with the modified email.
            setDemands(dem);
        }
    };
    const createRow = (count, count2, values, name) => {
        if (name === 'prices') {
            const rows = [];
            for (let i = 0; i < count; i++) {
                const row = [];
                for (let j = 0; j < count2; j++) {
                    row.push(<td><Input
                        key={i + j}
                        key_r={i}
                        key_c={j}
                        //value={values[i]}
                        onChange={(e) => handleArrayChange(e, i)}
                        name={name}
                    /></td>)
                }
                rows.push(<tr>{row}</tr>);
            }
            return rows
        } else {
            const row = [];
            for (let i = 0; i < count; i++) {
                row.push(<td><Input
                    key={i}
                    //value={values[i]}
                    onChange={(e) => handleArrayChange(e, i)}
                    name={name}
                /></td>)
            }
            return row;
        }
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
        </>
    );
};

InputTable.propTypes = {};
InputTable.defaultProps = {};

export default InputTable;
