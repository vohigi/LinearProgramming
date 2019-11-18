import React from 'react';
import PropTypes from 'prop-types';

const Result = ({array,value}) => {
    const createRow = (values) => {
            const rows = [];
            for (let i = 0; i < values.length; i++) {
                const row = [];
                for (let j = 0; j < values[0].length; j++) {
                    row.push(<td style={{"width":"160px","border":"1px solid black"}}>{array[i][j]}</td>)
                }
                rows.push(<tr>{row}</tr>);
            }
            return rows;
        };
    return (
        <div>
            <table>
                <tbody>
                {createRow(array)}
                </tbody>
            </table>
            <p>Cost: {value}</p>
        </div>
    );
};

Result.propTypes = {};
Result.defaultProps = {};

export default Result;
